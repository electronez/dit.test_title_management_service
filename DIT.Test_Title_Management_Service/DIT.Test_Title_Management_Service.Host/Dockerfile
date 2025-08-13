FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DIT.Test_Title_Management_Service.Host/DIT.Test_Title_Management_Service.Host.csproj", "DIT.Test_Title_Management_Service.Host/"]
COPY ["DIT.Test_Title_Management_Service.Persistence/DIT.Test_Title_Management_Service.Persistence.csproj", "DIT.Test_Title_Management_Service.Persistence/"]
COPY ["DIT.Test_Title_Management_Service.Application/DIT.Test_Title_Management_Service.Application.csproj", "DIT.Test_Title_Management_Service.Application/"]
COPY ["DIT.Test_Title_Management_Service.Domain/DIT.Test_Title_Management_Service.Domain.csproj", "DIT.Test_Title_Management_Service.Domain/"]
RUN dotnet restore "DIT.Test_Title_Management_Service.Host/DIT.Test_Title_Management_Service.Host.csproj"
COPY . .
WORKDIR "/src/DIT.Test_Title_Management_Service.Host"
RUN dotnet build "./DIT.Test_Title_Management_Service.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DIT.Test_Title_Management_Service.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DIT.Test_Title_Management_Service.Host.dll"]
