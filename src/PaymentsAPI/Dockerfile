# Imagem usada para executar a API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER app
WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development


# Imagem usada para restaurar e compilar o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["src/PaymentsAPI/PaymentsAPI.csproj", "src/PaymentsAPI/"]
COPY ["src/PaymentsAPI.Application/PaymentsAPI.Application.csproj", "src/PaymentsAPI.Application/"]
COPY ["src/PaymentsAPI.Domain/PaymentsAPI.Domain.csproj", "src/PaymentsAPI.Domain/"]
COPY ["src/PaymentsAPI.Infrastructure/PaymentsAPI.Infrastructure.csproj", "src/PaymentsAPI.Infrastructure/"]
COPY ["src/PaymentsAPI.Shared/PaymentsAPI.Shared.csproj", "src/PaymentsAPI.Shared/"]

RUN dotnet restore "src/PaymentsAPI/PaymentsAPI.csproj"

COPY . .

WORKDIR "/src/src/PaymentsAPI"

RUN dotnet build "PaymentsAPI.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build \
    --no-restore


# Publicação da aplicação
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "PaymentsAPI.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-restore


# Imagem final
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PaymentsAPI.dll"]