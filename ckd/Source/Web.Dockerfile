# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /Web
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image Web
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /Web
COPY --from=build-env /Web/out .
ENTRYPOINT ["dotnet", "Web.dll"]