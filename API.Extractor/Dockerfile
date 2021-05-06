#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["API.Extractor/API.Extractor.csproj", "API.Extractor/"]
RUN dotnet restore "API.Extractor/API.Extractor.csproj"
COPY . .
WORKDIR "/src/API.Extractor"
RUN dotnet build "API.Extractor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Extractor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Extractor.dll"]