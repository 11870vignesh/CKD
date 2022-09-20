# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /WebAPI
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image Web
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /WebAPI
COPY --from=build-env /WebAPI/out .
ENTRYPOINT ["dotnet", "Web.dll"]