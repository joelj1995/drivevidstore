#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DriveVidStore-Worker.csproj", ""]
RUN dotnet restore "./DriveVidStore-Worker.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DriveVidStore-Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DriveVidStore-Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY 3rd-party /3rd-party
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DriveVidStore-Worker.dll"]