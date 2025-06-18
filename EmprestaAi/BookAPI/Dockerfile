# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
WORKDIR /src/BookAPI
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Adicione esta linha para instalar ferramentas de desenvolvimento (incluindo Swagger)
RUN apt-get update && apt-get install -y curl
ENTRYPOINT ["dotnet", "BookAPI.dll"]