# Use the appropriate base image with .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory
WORKDIR /app

# Copy the .csproj and .sln files to the container
COPY *.csproj ./
COPY *.sln ./

# Restore dependencies
RUN dotnet restore

# Copy the remaining source code to the container
COPY . ./

# Build the application
RUN dotnet build -c Release -o out

# Publish the application
RUN dotnet publish -c Release -o /app/out

# Create a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published application from the build image
COPY --from=build /app/out ./

# Define the entry point for the application.
ENTRYPOINT ["dotnet", "BookingSystem.API.dll"]
