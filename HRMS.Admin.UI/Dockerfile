#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HRMS.Admin.UI/HRMS.Admin.UI.csproj", "HRMS.Admin.UI/"]
COPY ["HRMS.Services.Implementation/HRMS.Services.Implementation.csproj", "HRMS.Services.Implementation/"]
COPY ["HRMS.Services.Repository/HRMS.Services.Repository.csproj", "HRMS.Services.Repository/"]
COPY ["HRMS.Core.ReqRespVm/HRMS.Core.ReqRespVm.csproj", "HRMS.Core.ReqRespVm/"]
COPY ["HRMS.Core.Entities/HRMS.Core.Entities.csproj", "HRMS.Core.Entities/"]
COPY ["HRMS.Core.Helpers/HRMS.Core.Helpers.csproj", "HRMS.Core.Helpers/"]
RUN dotnet restore "HRMS.Admin.UI/HRMS.Admin.UI.csproj"
COPY . .
WORKDIR "/src/HRMS.Admin.UI"
RUN dotnet build "HRMS.Admin.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HRMS.Admin.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HRMS.Admin.UI.dll"]