#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GSES2.Api/GSES2.Api.csproj", "GSES2.Api/"]
COPY ["GSES2.Application/GSES2.Application.csproj", "GSES2.Application/"]
COPY ["GSES2.Core/GSES2.Core.csproj", "GSES2.Core/"]
COPY ["GSES2.Domain/GSES2.Domain.csproj", "GSES2.Domain/"]
COPY ["GSES2.Repository/GSES2.Repository.csproj", "GSES2.Repository/"]
RUN dotnet restore "GSES2.Api/GSES2.Api.csproj"
COPY . .
WORKDIR "/src/GSES2.Api"
RUN dotnet build "GSES2.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GSES2.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GSES2.Api.dll", "--environment=Development"]