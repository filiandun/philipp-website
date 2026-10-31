# build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["PhilippWebsite.Server/PhilippWebsite.Server.csproj", "PhilippWebsite.Server/"]
COPY ["PhilippWebsite.Shared/PhilippWebsite.Shared.csproj", "PhilippWebsite.Shared/"]

RUN dotnet restore "PhilippWebsite.Server/PhilippWebsite.Server.csproj"

COPY . .
WORKDIR "/src/PhilippWebsite.Server"

RUN dotnet publish "PhilippWebsite.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PhilippWebsite.Server.dll"]