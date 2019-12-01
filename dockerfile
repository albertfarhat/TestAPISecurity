FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["TestAPISecurity/TestAPISecurity.csproj", "TestAPISecurity/"]
RUN dotnet restore "TestAPISecurity/TestAPISecurity.csproj"
COPY . .
WORKDIR .
RUN dotnet build "TestAPISecurity/TestAPISecurity.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TestAPISecurity/TestAPISecurity.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestSecurityAPI.dll"]