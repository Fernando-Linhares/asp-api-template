FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV APP_ENV=host

COPY --from=build-env /app/out .
COPY --from=build-env /app/.env .
COPY --from=build-env /app/storage/public ./storage/public
COPY --from=build-env /app/templates ./templates

RUN mkdir wwwroot/
RUN ln -s /app/storage/public /app/wwwroot/public

EXPOSE 8080

ENTRYPOINT ["dotnet", "api-asp.dll"]
