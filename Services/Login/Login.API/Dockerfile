#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Login/Login.API/Login.API.csproj", "Services/Login/Login.API/"]
RUN dotnet restore "Services/Login/Login.API/Login.API.csproj"
COPY . .
WORKDIR "/src/Services/Login/Login.API"
RUN dotnet build "Login.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Login.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Login.API.dll"]