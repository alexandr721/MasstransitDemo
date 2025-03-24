# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Установка переменных
ARG ROOT_DIR="Demo.AuthService"
ARG PROJECT_NAME="AuthService"
ARG PROJECT_PATH=$ROOT_DIR/$PROJECT_NAME/$PROJECT_NAME.csproj
ARG USE_DOCKER_NUGET=true
ENV USE_DOCKER_NUGET=$USE_DOCKER_NUGET


# Копируем весь проект
WORKDIR /src
COPY . .
COPY $ROOT_DIR/NuGet.config.docker $ROOT_DIR/NuGet.config
COPY nuget-packages/. /root/.nuget/local-packages

# Устанавливаем локальный NuGet source
RUN dotnet nuget add source /root/.nuget/local-packages --name Local --configfile NuGet.Config || true

# Восстановление зависимостей с указанием переменной USE_DOCKER_NUGET
RUN dotnet restore $PROJECT_PATH -p:USE_DOCKER_NUGET=true


# Сборка
RUN dotnet build $PROJECT_PATH -c Release -o /app/build -p:USE_DOCKER_NUGET=true

# Публикация
RUN dotnet publish $PROJECT_PATH -c Release -o /app/publish -p:USE_DOCKER_NUGET=true

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Открываем порт (если нужен)
# EXPOSE 80

# Точка входа
ENTRYPOINT ["dotnet", "AuthService.dll"]