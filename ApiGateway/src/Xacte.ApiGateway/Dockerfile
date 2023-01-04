#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["ApiGateway/src/Xacte.ApiGateway/Xacte.ApiGateway.csproj", "ApiGateway/src/Xacte.ApiGateway/"]
RUN dotnet restore "ApiGateway/src/Xacte.ApiGateway/Xacte.ApiGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateway/src/Xacte.ApiGateway"
RUN dotnet build "Xacte.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Xacte.ApiGateway.csproj" -c Release -o /app/publish --os linux --arch x64 /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Xacte.ApiGateway.dll"]