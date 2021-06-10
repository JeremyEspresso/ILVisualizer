#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/ILVisualizer.Bot/ILVisualizer.Bot.csproj", "src/ILVisualizer.Bot/"]
COPY ["src/ILVisualizer.Application/ILVisualizer.Application.csproj", "src/ILVisualizer.Application/"]
COPY ["src/ILVisualizer.Domain/ILVisualizer.Domain.csproj", "src/ILVisualizer.Domain/"]
RUN dotnet restore "src/ILVisualizer.Bot/ILVisualizer.Bot.csproj"
COPY . .
WORKDIR "/src/src/ILVisualizer.Bot"
RUN dotnet build "ILVisualizer.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ILVisualizer.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ILVisualizer.Bot.dll"]
