FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8893

FROM node:20-alpine as build-deps
WORKDIR /src
COPY package.json package-lock.json ./
RUN npm i -g gulp-cli
RUN npm i
COPY . ./
RUN gulp

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG BAGET_API_KEY
WORKDIR /src
COPY ["src/Host/Host.csproj", "src/Host/"]
RUN dotnet nuget add source --username oliverbooth --password $BAGET_API_KEY --store-password-in-clear-text --name baget "https://nuget.booth.dev/v3/index.json"
RUN dotnet restore "src/Host/Host.csproj"
COPY . .
WORKDIR "/src/src/Host"
RUN dotnet build "Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Host.dll"]
