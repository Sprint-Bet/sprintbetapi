#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

RUN curl -L https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh  | sh

# Copy csproj and restore as distinct layers
COPY ["SprintBetApi/SprintBetApi.csproj", "SprintBetApi/"]
#COPY ["Into.Enrollment.ApplicationFormService.Api/Into.Enrollment.ApplicationFormService.Api.csproj", "Into.Enrollment.ApplicationFormService.Api/"]
# COPY ./nuget.config .
# -- Not needed!!! --
#ARG SYSTEM_ACCESSTOKEN
#ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/iup2/Into.Enrollment/_packaging/Into.Enrollment/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"${SYSTEM_ACCESSTOKEN}\"}]}"
# -- Not needed!!! --
RUN dotnet restore "SprintBetApi/SprintBetApi.csproj"
# RUN dotnet restore "Into.Enrollment.ApplicationFormService.Api/Into.Enrollment.ApplicationFormService.Api.csproj"

COPY . .
WORKDIR "/src/SprintBetApi"
# WORKDIR "/src/Into.Enrollment.ApplicationFormService.Api"
RUN dotnet build "SprintBetApi.csproj" -c Release -o /app/build
# RUN dotnet build "Into.Enrollment.ApplicationFormService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SprintBetApi.csproj" -c Release -o /app/publish
# RUN dotnet publish "Into.Enrollment.ApplicationFormService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SprintBetApi.dll"]