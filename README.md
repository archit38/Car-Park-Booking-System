# Car Park Booking System

The Car Park Booking System is a web API for managing car park bookings. It allows customers to create, view, modify, and cancel bookings for parking spaces.

## Getting Started

Follow these steps to set up and run the Car Park Booking System API on your local machine.

### Prerequisites

Before you begin, ensure you have met the following requirements:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio Code](https://code.visualstudio.com/) or another code editor of your choice
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/CarParkBookingSystem.git

2. In the appsettings.json file, configure your database connection string:

    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=localhost,1433;Database=CarPark;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

### Usage
Build and run unit tests using the UnitTests project

```
cd UnitTests
dotnet test
```

Run the solution:
Access the API at http://localhost:5000 (or https://localhost:5001 for HTTPS).

#### API Endpoints
- GET /api/carpark/GetAllBookings: Get a list of all bookings.
- GET /api/carpark/GetBookingById/{id}: Get a booking by ID.
- POST /api/carpark/CreateBooking: Create a new booking. Requires JSON payload with customerName, from, and to fields.
- PUT /api/carpark/AmendBooking/{bookingId}: Amend an existing booking. Requires JSON payload with newFrom and newTo fields.
- DELETE /api/carpark/CancelBooking/{id}: Cancel a booking by ID.
- GET /api/carpark/GetPrice: Get the price for a date range. Requires from and to query parameters.
- GET /api/carpark/CheckAvailability: Check parking space availability for a date range. Requires from and to query parameters.


#### Rate Limiting
The API includes rate limiting to prevent abuse. Rate limiting settings can be configured in the app settings.

#### Logging
Logging is implemented using the built-in Console logger. Extend logging capabilities as needed.
