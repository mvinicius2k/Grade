##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["Grade/Grade.csproj", "Grade/"]
#RUN dotnet restore "Grade/Grade.csproj"
#
#COPY . .
#WORKDIR "/src/Grade"
#RUN dotnet build "Grade.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Grade.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Grade.dll"]
#

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Grade/Grade.csproj", "Grade/"]
RUN dotnet restore "Grade/Grade.csproj"

COPY . .
WORKDIR "/src/Grade"
RUN dotnet build "Grade.csproj" -c Release -o /app/build

# Descomente este bloco para aplicar o update de migração
#RUN dotnet tool install --global dotnet-ef
#ENV PATH="$PATH:/root/.dotnet/tools"
#RUN dotnet ef database update

FROM build AS publish
RUN dotnet publish "Grade.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Grade.dll"]