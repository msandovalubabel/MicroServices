#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Servicios.api.libreria/Servicios.api.libreria.csproj", "Servicios.api.libreria/"]
COPY ["Servicios.RabbitMQ.Bus/Servicios.RabbitMQ.Bus.csproj", "Servicios.RabbitMQ.Bus/"]
RUN dotnet restore "Servicios.api.libreria/Servicios.api.libreria.csproj"
COPY . .
WORKDIR "/src/Servicios.api.libreria"
RUN dotnet build "Servicios.api.libreria.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Servicios.api.libreria.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Servicios.api.libreria.dll"]