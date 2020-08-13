#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/AirportService/AirportService.App/AirportService.App.csproj", "Services/AirportService/AirportService.App/"]
COPY ["Services/AirportService/AirportService.BLL/AirportService.BLL.csproj", "Services/AirportService/AirportService.BLL/"]
COPY ["Infrastructure/Shared/Shared.csproj", "Infrastructure/Shared/"]
COPY ["Infrastructure/Contracts/Contracts.csproj", "Infrastructure/Contracts/"]
COPY ["Infrastructure/Components/Components.csproj", "Infrastructure/Components/"]
RUN dotnet restore "Services/AirportService/AirportService.App/AirportService.App.csproj"
COPY . .
WORKDIR "/src/Services/AirportService/AirportService.App"
RUN dotnet build "AirportService.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AirportService.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AirportService.App.dll"]