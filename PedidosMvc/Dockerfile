FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENV DOTNET_EnableDiagnostics=0
EXPOSE $PORT
ENTRYPOINT ["dotnet", "PedidosMvc.dll", "ASPNETCORE_URLS=http://*:$PORT"]