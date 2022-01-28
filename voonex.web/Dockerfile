FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["voonex.web/voonex.web.csproj", "voonex.web/"]
RUN dotnet restore "voonex.web/voonex.web.csproj"
COPY . .
WORKDIR "/src/voonex.web"
RUN dotnet build "voonex.web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "voonex.web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "voonex.web.dll"]
