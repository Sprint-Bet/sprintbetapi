# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

RUN curl -L https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh  | sh

# Copy csproj and restore as distinct layers
COPY ["SprintBetApi/SprintBetApi.csproj", "SprintBetApi/"]
RUN dotnet restore "SprintBetApi/SprintBetApi.csproj"

COPY . .
WORKDIR "/src/SprintBetApi"
RUN dotnet build "SprintBetApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SprintBetApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SprintBetApi.dll"]