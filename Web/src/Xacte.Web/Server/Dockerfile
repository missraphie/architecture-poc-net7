#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Web/src/Xacte.Web/Server/Xacte.Web.Server.csproj", "Web/src/Xacte.Web/Server/"]
COPY ["Web/src/Xacte.Web/Client/Xacte.Web.Client.csproj", "Web/src/Xacte.Web/Client/"]
COPY ["Web/src/Xacte.Web/Shared/Xacte.Web.Shared.csproj", "Web/src/Xacte.Web/Shared/"]
RUN dotnet restore "Web/src/Xacte.Web/Server/Xacte.Web.Server.csproj"
COPY . .
WORKDIR "/src/Web/src/Xacte.Web/Server"
RUN dotnet build "Xacte.Web.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Xacte.Web.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Xacte.Web.Server.dll"]