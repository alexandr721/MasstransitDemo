import os
import re
import shutil
import subprocess
from pathlib import Path

root_dir = Path(__file__).resolve().parent
shared_proj = root_dir / "Demo.Shared" / "Demo.Shared.csproj"
nuget_dir = root_dir / "nuget-packages"

print("Starting build and publish process...")
print(root_dir)

def bump_version(csproj_path):
    content = csproj_path.read_text(encoding="utf-8")
    match = re.search(r"<Version>(\d+)\.(\d+)\.(\d+)</Version>", content)
    if not match:
        raise Exception("Version tag not found in csproj.")

    major, minor, patch = map(int, match.groups())
    patch += 1
    new_version = f"{major}.{minor}.{patch}"
    new_content = re.sub(r"<Version>.*?</Version>", f"<Version>{new_version}</Version>", content)
    csproj_path.write_text(new_content, encoding="utf-8")
    print(f"Version updated to: {new_version}")
    return new_version

def run_cmd(cmd, cwd=None):
    result = subprocess.run(cmd, cwd=cwd, shell=True, capture_output=True, text=True)
    clean_out = re.sub(r'[^\x20-\x7E\n]', '', result.stdout)
    clean_err = re.sub(r'[^\x20-\x7E\n]', '', result.stderr)
    print(clean_out)
    if result.returncode != 0:
        print(clean_err)
        raise Exception(f"Command failed: {cmd}")
    return clean_out

def clean_and_restore(project_path):
    print(f"clean_and_restore:{project_path}")
    for folder in ["bin", "obj"]:
        full_path = project_path / folder
        if full_path.exists():
            shutil.rmtree(full_path)
    print(f"{project_path}")
    print(f"{root_dir}")
    project = f"{project_path}".replace(f"{root_dir}\\","")
    cmd = f"dotnet restore --no-cache {project} --configfile NuGet.config"
    print(f"RESTORE: {cmd}")
    run_cmd(cmd, cwd=root_dir)

new_version = bump_version(shared_proj)

run_cmd("dotnet restore", cwd=shared_proj.parent)
run_cmd("dotnet build -c Release", cwd=shared_proj.parent)
run_cmd("dotnet pack -c Release", cwd=shared_proj.parent)

# Копируем пакет в nuget-packages
nupkg_name = f"Demo.Shared.{new_version}.nupkg"
src_nupkg = shared_proj.parent / "bin" / "Release" / nupkg_name
dst_nupkg = nuget_dir / nupkg_name
dst_nupkg.parent.mkdir(parents=True, exist_ok=True)
shutil.copy2(src_nupkg, dst_nupkg)
print(f"Copied {nupkg_name} to nuget-packages")

# Обновляем все зависимости в проектах
for csproj_path in root_dir.rglob("*.csproj"):
    if csproj_path.name == "Demo.Shared.csproj":
        continue
    text = csproj_path.read_text(encoding="utf-8")
    updated = re.sub(r'<PackageReference Include="Demo\.Shared" Version=".*?" />',
                     f'<PackageReference Include="Demo.Shared" Version="{new_version}" />',
                     text)
    csproj_path.write_text(updated, encoding="utf-8")
    print(f"Updated Demo.Shared version in {csproj_path}")

    clean_and_restore(csproj_path)

print("Done.")
