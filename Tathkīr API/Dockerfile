# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy any csproj 
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the source
COPY . ./

# Publish with a safe ASCII assembly name to avoid any quirks
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false /p:AssemblyName=Tathkir.API

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "Tathkir.API.dll"]
