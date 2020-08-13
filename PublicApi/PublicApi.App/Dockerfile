#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PublicApi/PublicApi.App/PublicApi.App.csproj", "PublicApi/PublicApi.App/"]
COPY ["PublicApi/PublicApi.BLL/PublicApi.BLL.csproj", "PublicApi/PublicApi.BLL/"]
COPY ["Infrastructure/Shared/Shared.csproj", "Infrastructure/Shared/"]
COPY ["Infrastructure/Contracts/Contracts.csproj", "Infrastructure/Contracts/"]
COPY ["Infrastructure/Components/Components.csproj", "Infrastructure/Components/"]
RUN dotnet restore "PublicApi/PublicApi.App/PublicApi.App.csproj"
COPY . .
WORKDIR "/src/PublicApi/PublicApi.App"
RUN dotnet build "PublicApi.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PublicApi.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicApi.App.dll"]
