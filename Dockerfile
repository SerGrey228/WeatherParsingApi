FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY /. ./
WORKDIR /app/
RUN dotnet publish -c Debug -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "WeatherApi.dll", "--environment=Development", "--urls=http://0.0.0.0:5000/"]

