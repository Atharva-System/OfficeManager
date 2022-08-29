#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/OfficeManager.API/OfficeManager.API.csproj", "src/OfficeManager.API/"]
COPY ["src/OfficeManager.Application/OfficeManager.Application.csproj", "src/OfficeManager.Application/"]
COPY ["src/OfficeManager.Domain/OfficeManager.Domain.csproj", "src/OfficeManager.Domain/"]
COPY ["src/OfficeManager.Infrastructure/OfficeManager.Infrastructure.csproj", "src/OfficeManager.Infrastructure/"]
RUN dotnet restore "src/OfficeManager.API/OfficeManager.API.csproj"
COPY . .
WORKDIR "/src/src/OfficeManager.API"
RUN dotnet build "OfficeManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OfficeManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OfficeManager.API.dll"]