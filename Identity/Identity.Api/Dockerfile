#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Identity/Identity.Api/Identity.Api.csproj", "Identity/Identity.Api/"]
COPY ["Common/Common.Logging/Common.Logging.csproj", "Common/Common.Logging/"]
COPY ["Identity/Identity.Service.Queries/Identity.Service.Queries.csproj", "Identity/Identity.Service.Queries/"]
COPY ["Common/Service.Common.Mapping/Service.Common.Mapping.csproj", "Common/Service.Common.Mapping/"]
COPY ["Common/Service.Common.Paging/Service.Common.Paging.csproj", "Common/Service.Common.Paging/"]
COPY ["Common/Service.Common.Collection/Service.Common.Collection.csproj", "Common/Service.Common.Collection/"]
COPY ["Identity/Identity.Persistence.Database/Identity.Persistence.Database.csproj", "Identity/Identity.Persistence.Database/"]
COPY ["Identity/Identity.Domain/Identity.Domain.csproj", "Identity/Identity.Domain/"]
COPY ["Identity/Identity.Service.EventHandlers/Identity.Service.EventHandlers.csproj", "Identity/Identity.Service.EventHandlers/"]
RUN dotnet restore "Identity/Identity.Api/Identity.Api.csproj"
COPY . .
WORKDIR "/src/Identity/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]