FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["voonex.api/voonex.api.csproj", "voonex.api/"]
RUN dotnet restore "voonex.api/voonex.api.csproj"
COPY . .
WORKDIR "/src/voonex.api"
RUN dotnet build "voonex.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "voonex.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "voonex.api.dll"]
