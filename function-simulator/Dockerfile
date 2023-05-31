# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["function-simulator/function-simulator.csproj", "function-simulator/"]
RUN dotnet restore "function-simulator/function-simulator.csproj"
COPY . .
WORKDIR "/src/function-simulator"
RUN dotnet build "function-simulator.csproj" -c Release -o /app/build

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/build .

# Set the entry point to your application
ENTRYPOINT ["dotnet", "function-simulator.dll"]