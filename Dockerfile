FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
 
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/CatsCRUDApp/CatsCRUDApp.csproj", "src/CatsCRUDApp/"]
COPY ["/CatsCRUDAppTest/CatsCRUDAppTest.csproj", "src/CatsCRUDAppTest/"]
COPY ["/DAL/DAL.csproj", "src/DAL/"]
COPY ["/BLL/BLL.csproj", "src/BLL/"]
RUN dotnet restore "src/CatsCRUDApp/CatsCRUDApp.csproj"
COPY . .
WORKDIR "/src/CatsCRUDApp"
RUN dotnet build "CatsCRUDApp.csproj" -c Release -o /app/build
 
FROM build AS publish
RUN dotnet publish "CatsCRUDApp.csproj" -c Release -o /app/publish
 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatsCRUDApp.dll"]