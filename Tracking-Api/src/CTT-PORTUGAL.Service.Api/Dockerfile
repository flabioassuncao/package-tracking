FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 60887
EXPOSE 44338

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/CTT-PORTUGAL.Service.Api/CTT-PORTUGAL.Service.Api.csproj", "src/CTT-PORTUGAL.Service.Api/"]
RUN dotnet restore "src/CTT-PORTUGAL.Service.Api/CTT-PORTUGAL.Service.Api.csproj"
COPY . .
WORKDIR "/src/src/CTT-PORTUGAL.Service.Api"
RUN dotnet build "CTT-PORTUGAL.Service.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CTT-PORTUGAL.Service.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CTT-PORTUGAL.Service.Api.dll"]