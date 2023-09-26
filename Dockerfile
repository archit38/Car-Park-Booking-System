# Use the appropriate base image with .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory
WORKDIR /app

# Copy the solution file to the container
COPY ParkingService.sln ./

# Copy the project files from each project folder
COPY BookingSystem.Data/*.csproj ./BookingSystem.Data/
COPY BookingSystem.Services/*.csproj ./BookingSystem.Services/
COPY BookingSystem.API/*.csproj ./BookingSystem.API/
# Repeat this for each project folder

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

# Specify the entry point for your application
ENTRYPOINT ["dotnet", "BookingSystem.API.dll"]
