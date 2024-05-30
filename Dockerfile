# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Ecommerce.WebApi/*.csproj ./Ecommerce.WebApi/
COPY Ecommerce.Core/*.csproj ./Ecommerce.Core/
COPY Ecommerce.Service/*.csproj ./Ecommerce.Service/
COPY Ecommerce.Controller/*.csproj ./Ecommerce.Controller/
#COPY Ecommerce.Tests/*.csproj ./Ecommerce.Tests/

RUN dotnet restore

# copy everything else and build app
COPY Ecommerce.WebApi/. ./Ecommerce.WebApi/
COPY Ecommerce.Core/. ./Ecommerce.Core/
COPY Ecommerce.Service/. ./Ecommerce.Service/
COPY Ecommerce.Controller/. ./Ecommerce.Controller/
#COPY Ecommerce.Tests/. ./Ecommerce.Tests/
WORKDIR /Ecommerce.WebApi
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "Ecommerce.WebApi.dll"]