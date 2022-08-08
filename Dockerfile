FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TradeArtTestProject/TradeArtTestProject.csproj", "TradeArtTestProject/"]
RUN dotnet restore "TradeArtTestProject/TradeArtTestProject.csproj"
COPY . .
WORKDIR "/src/TradeArtTestProject"
RUN dotnet build "TradeArtTestProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradeArtTestProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradeArtTestProject.dll", "--launch-profile TradeArtTestProject"]