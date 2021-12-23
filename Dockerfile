FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env
WORKDIR /app
COPY . .
RUN dotnet restore --source https://api.nuget.org/v3/index.json --source http://nuget/v3/index.json
RUN dotnet publish HomeAutomationApi.WebApi/HomeAutomationApi.WebApi.csproj --configuration Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
EXPOSE 80/tcp
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "HomeAutomationApi.WebApi.dll"]
