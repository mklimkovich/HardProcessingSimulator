# Use .NET 7 base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# Set the working directory to /app inside the container
WORKDIR /source
# ==== BUILD =====
# Install dependencies
COPY *.csproj .
RUN dotnet restore
# Copy all files
COPY . .
# Build the app
RUN dotnet publish -c release -o /app --no-restore
# ==== RUN =======
FROM mcr.microsoft.com/dotnet/aspnet:7.0
# Expose the port on which the app will be running
EXPOSE 80
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "webapi.dll"]