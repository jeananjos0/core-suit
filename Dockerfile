# Imagem base com .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copia os csproj e restaura as dependências
COPY Backend.sln ./
COPY CoreSuit.Domain/*.csproj ./CoreSuit.Domain/
COPY CoreSuit.Application/*.csproj ./CoreSuit.Application/
COPY CoreSuit.Infrastructure/*.csproj ./CoreSuit.Infrastructure/
COPY CoreSuit.CrossCutting/*.csproj ./CoreSuit.CrossCutting/
COPY CoreSuit.API/*.csproj ./CoreSuit.API/
RUN dotnet restore

# Copia os arquivos do projeto e constrói
COPY . ./
RUN dotnet publish CoreSuit.API -c Release -o out

# Gera a imagem de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Define a variável de ambiente para desativar o recarregamento de configuração
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CoreSuit.API.dll"]