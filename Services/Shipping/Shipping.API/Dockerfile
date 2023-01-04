#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Shipping/Shipping.API/Shipping.API.csproj", "Services/Shipping/Shipping.API/"]
RUN dotnet restore "Services/Shipping/Shipping.API/Shipping.API.csproj"
COPY . .
WORKDIR "/src/Services/Shipping/Shipping.API"
RUN dotnet build "Shipping.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shipping.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shipping.API.dll"]