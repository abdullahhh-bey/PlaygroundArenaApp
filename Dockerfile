# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore first (better layer caching)
COPY PlaygroundArenaApp/PlaygroundArenaApp.csproj PlaygroundArenaApp/
RUN dotnet restore "PlaygroundArenaApp/PlaygroundArenaApp.csproj"

# Copy everything and publish
COPY . .
WORKDIR /src/PlaygroundArenaApp
RUN dotnet publish "PlaygroundArenaApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render/most PaaS inject PORT; default to 8080 locally
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "PlaygroundArenaApp.dll"]
