# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiază totul (să includă Application, Domain, Infrastructure etc.)
COPY . .

# Restaurează pachetele
RUN dotnet restore "AdWriter-AI/AdWriter-AI.csproj"

# Publică proiectul de API
RUN dotnet publish "AdWriter-AI/AdWriter-AI.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AdWriter-AI.dll"]
