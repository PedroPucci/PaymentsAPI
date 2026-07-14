FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER app
WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_HTTP_PORTS=8080


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["nuget.config", "/src/nuget.config"]
COPY ["nuget-packages/", "/src/nuget-packages/"]

COPY ["src/PaymentsAPI/PaymentsAPI.csproj", "src/PaymentsAPI/"]
COPY ["src/PaymentsAPI.Application/PaymentsAPI.Application.csproj", "src/PaymentsAPI.Application/"]
COPY ["src/PaymentsAPI.Domain/PaymentsAPI.Domain.csproj", "src/PaymentsAPI.Domain/"]
COPY ["src/PaymentsAPI.Infrastructure/PaymentsAPI.Infrastructure.csproj", "src/PaymentsAPI.Infrastructure/"]
COPY ["src/PaymentsAPI.Shared/PaymentsAPI.Shared.csproj", "src/PaymentsAPI.Shared/"]

RUN dotnet restore "src/PaymentsAPI/PaymentsAPI.csproj" \
    --configfile "/src/nuget.config"

COPY . .

RUN dotnet publish "src/PaymentsAPI/PaymentsAPI.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false


FROM base AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PaymentsAPI.dll"]