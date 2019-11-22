FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS build
WORKDIR /src
COPY ["src/ApiGateway/ApiGateway.csproj", "src/gateways/ApiGateway/"]
RUN dotnet restore "src/gateways/ApiGateway/ApiGateway.csproj"
COPY . .
WORKDIR "/src/gateways/ApiGateway"
RUN dotnet build "ApiGateway.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]