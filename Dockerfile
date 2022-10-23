FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore GhostNetwork.Cockpit/GhostNetwork.Cockpit.csproj
WORKDIR /src/GhostNetwork.Cockpit
RUN dotnet build GhostNetwork.Cockpit.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GhostNetwork.Cockpit.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GhostNetwork.Cockpit.dll"]
