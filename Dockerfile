# Utiliser l'image officielle .NET pour le runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Utiliser l'image SDK pour construire l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier les fichiers de solution et les projets
COPY ["ReserGo.WebApi.sln", "./"]
COPY ["ReserGo.WebApi/ReserGo.WebApi.csproj", "ReserGo.WebApi/"]
COPY ["ReserGo.Business/ReserGo.Business.csproj", "ReserGo.Business/"]
COPY ["ReserGo.Common/ReserGo.Common.csproj", "ReserGo.Common/"]
COPY ["ReserGo.DataAccess/ReserGo.DataAccess.csproj", "ReserGo.DataAccess/"]
COPY ["ReserGo.Shared/ReserGo.Shared.csproj", "ReserGo.Shared/"]
COPY ["ReserGo.Tiers/ReserGo.Tiers.csproj", "ReserGo.Tiers/"]

# Si vous avez besoin du projet de test
COPY ["ReserGo.WebApi.Tests/ReserGo.WebApi.Tests.csproj", "ReserGo.WebApi.Tests/"]

# Restaurer les dépendances
RUN dotnet restore "./ReserGo.WebApi.sln"

# Copier le reste des fichiers
COPY . .

# Construire l'application
RUN dotnet build "ReserGo.WebApi.sln" -c Release -o /app/build

# Publier l'application
FROM build AS publish
RUN dotnet publish "ReserGo.WebApi/ReserGo.WebApi.csproj" -c Release -o /app/publish

# Créer l'image finale
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReserGo.WebApi.dll"]
