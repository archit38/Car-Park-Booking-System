# Use the official .NET SDK image as a build stage.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory to /app.
WORKDIR /app

# Copy the .csproj and .sln files and restore any dependencies (if applicable).
COPY *.csproj ./
COPY *.sln ./
RUN dotnet restore

# Copy the remaining source code to the container.
COPY . .

# Build the application in release mode.
RUN dotnet publish -c Release -o out

# Use a smaller runtime image for the final stage.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory to /app.
WORKDIR /app

# Copy the published application from the build stage.
COPY --from=build /app/out .

# Expose port 80 for the web application.
EXPOSE 80

# Define the entry point for the application.
ENTRYPOINT ["dotnet", "BookingSystem.API.dll"]
