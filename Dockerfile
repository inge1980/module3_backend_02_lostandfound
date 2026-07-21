FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore "webapi/webapi.csproj"

RUN dotnet publish "webapi/webapi.csproj" \
    -c Release \
    -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "webapi.dll"]