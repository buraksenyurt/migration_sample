#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ClassicGames.WebClient/ClassicGames.WebClient.csproj", "ClassicGames.WebClient/"]
COPY ["ClassicGames.DAL/ClassicGames.DAL.csproj", "ClassicGames.DAL/"]
COPY ["ClassicGames.Models/ClassicGames.Models.csproj", "ClassicGames.Models/"]
RUN dotnet restore "ClassicGames.WebClient/ClassicGames.WebClient.csproj"
COPY . .
WORKDIR "/src/ClassicGames.WebClient"
RUN dotnet build "ClassicGames.WebClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClassicGames.WebClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClassicGames.WebClient.dll"]
