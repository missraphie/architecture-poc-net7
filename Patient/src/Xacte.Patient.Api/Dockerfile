#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Patient/src/Xacte.Patient.Api/Xacte.Patient.Api.csproj", "Patient/src/Xacte.Patient.Api/"]
RUN dotnet restore "Patient/src/Xacte.Patient.Api/Xacte.Patient.Api.csproj"
COPY . .
WORKDIR "/src/Patient/src/Xacte.Patient.Api"
RUN dotnet build "Xacte.Patient.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Xacte.Patient.Api.csproj" -c Release -o /app/publish --os linux --arch x64 /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Xacte.Patient.Api.dll"]