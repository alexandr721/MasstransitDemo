# Stage 1 � Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# ������� ����� ��� ��������� �������
RUN mkdir -p /packages

# �������� ��������� NuGet ������
COPY ./nuget-packages/*.nupkg /packages/
#RUN ls -la /packages && exit 1
# ��������� ��������� ����� ��� �������� NuGet
RUN dotnet nuget add source /packages --name local-packages

WORKDIR /src

ARG ROOT_DIR=NotificationSender/NotificationTelegramSender
ARG PROJECT_NAME=NotificationTelegramSender
ARG PROJECT_PATH=$ROOT_DIR/$PROJECT_NAME/$PROJECT_NAME.csproj

# �������� ���������
COPY . .

# �������������� ������������
RUN dotnet restore $PROJECT_PATH \
    -p:USE_DOCKER_NUGET=true

# ������ � ����������
RUN dotnet build $PROJECT_PATH -c Release -o /app/build
RUN dotnet publish $PROJECT_PATH -c Release -o /app/publish

# Stage 2 � Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "NotificationTelegramSender.dll"]
