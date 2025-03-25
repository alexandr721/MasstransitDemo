# Stage 1 — Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# Создаем папку для локальных пакетов
RUN mkdir -p /packages

# Копируем локальные NuGet пакеты
COPY ./nuget-packages/*.nupkg /packages/
#RUN ls -la /packages && exit 1
# Добавляем локальную папку как источник NuGet
RUN dotnet nuget add source /packages --name local-packages

WORKDIR /src

ARG ROOT_DIR=NotificationSender/NotificationEmailSender
ARG PROJECT_NAME=NotificationEmailSender
ARG PROJECT_PATH=$ROOT_DIR/$PROJECT_NAME/$PROJECT_NAME.csproj

# Копируем исходники
COPY . .

# Восстановление зависимостей
RUN dotnet restore $PROJECT_PATH \
    -p:USE_DOCKER_NUGET=true

# Сборка и публикация
RUN dotnet build $PROJECT_PATH -c Release -o /app/build
RUN dotnet publish $PROJECT_PATH -c Release -o /app/publish

# Stage 2 — Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "NotificationEmailSender.dll"]
