# Hotel Booking Application

A full-stack hotel booking application built with Angular frontend and .NET 8 backend.

## Project Structure

```
├── backend/                 # .NET 8 ASP.NET Core Web API
│   └── HotelBookingAPI/    # Main API project
│       ├── Controllers/    # API Controllers
│       ├── Models/         # Entity Models
│       ├── DTOs/           # Data Transfer Objects
│       ├── Data/           # Database Context
│       └── Services/       # Business Logic Services
│
└── frontend/               # Angular Application
    └── src/
        └── app/
            ├── components/ # Angular Components
            ├── services/   # API Services
            ├── models/     # TypeScript Interfaces
            └── interceptors/ # HTTP Interceptors
```

## Features

### Backend (.NET 8)
- RESTful API with ASP.NET Core Web API
- Entity Framework Core for database operations
- JWT authentication and authorization
- Swagger/OpenAPI documentation
- CORS configuration for Angular frontend
- Models: Users, Hotels, Rooms, Bookings, Reviews
- Complete CRUD operations for all entities

### Frontend (Angular)
- Latest Angular LTS version
- TypeScript with strict mode
- Bootstrap 5 for styling
- Component-based architecture
- Services for API communication
- HTTP interceptors for authentication and error handling
- Responsive design

### Core Functionality
1. **User Management**
   - User registration and login
   - JWT token-based authentication
   - User profile management

2. **Hotel Management**
   - Browse hotels with filters (city, country, star rating)
   - View hotel details
   - Hotel reviews and ratings

3. **Room Management**
   - View available rooms per hotel
   - Room details with amenities
   - Room availability checking

4. **Booking System**
   - Create new bookings
   - View booking history
   - Cancel bookings
   - Booking status management (Pending, Confirmed, Cancelled, Completed)

5. **Review System**
   - Submit hotel reviews
   - View hotel ratings
   - Review approval system

## Prerequisites

### Backend
- .NET 8 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 or VS Code (optional)

### Frontend
- Node.js (v18 or later)
- npm or yarn
- Angular CLI

## Setup Instructions

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd backend/HotelBookingAPI
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Update the connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HotelBookingDB;Trusted_Connection=true;TrustServerCertificate=true"
   }
   ```

4. Create and apply database migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. Run the API:
   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:5001` or `http://localhost:5000`

6. Access Swagger documentation at `https://localhost:5001` or `http://localhost:5000`

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Update API URL if needed in service files (default is `http://localhost:5000`):
   - `src/app/services/*.service.ts`

4. Run the development server:
   ```bash
   npm start
   ```

   The application will be available at `http://localhost:4200`

## Default Users

The application includes seeded users for testing:

**Admin User:**
- Email: admin@hotelbooking.com
- Password: Admin@123

**Regular User:**
- Email: john.doe@example.com
- Password: User@123

## API Endpoints

### Authentication
- `POST /api/Users/register` - Register new user
- `POST /api/Users/login` - Login user

### Hotels
- `GET /api/Hotels` - Get all hotels (with optional filters)
- `GET /api/Hotels/{id}` - Get hotel by ID
- `POST /api/Hotels` - Create new hotel
- `PUT /api/Hotels/{id}` - Update hotel
- `DELETE /api/Hotels/{id}` - Delete hotel (soft delete)

### Rooms
- `GET /api/Rooms` - Get all rooms (with optional hotelId filter)
- `GET /api/Rooms/{id}` - Get room by ID
- `POST /api/Rooms/CheckAvailability` - Check room availability
- `POST /api/Rooms` - Create new room
- `PUT /api/Rooms/{id}` - Update room
- `DELETE /api/Rooms/{id}` - Delete room

### Bookings
- `GET /api/Bookings` - Get all bookings (with optional userId filter)
- `GET /api/Bookings/{id}` - Get booking by ID
- `POST /api/Bookings` - Create new booking
- `PUT /api/Bookings/{id}` - Update booking
- `POST /api/Bookings/{id}/cancel` - Cancel booking
- `POST /api/Bookings/{id}/confirm` - Confirm booking

### Reviews
- `GET /api/Reviews` - Get all reviews (with optional filters)
- `GET /api/Reviews/{id}` - Get review by ID
- `POST /api/Reviews` - Create new review
- `PUT /api/Reviews/{id}` - Update review
- `DELETE /api/Reviews/{id}` - Delete review
- `POST /api/Reviews/{id}/approve` - Approve review (admin)

## Technologies Used

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server
- JWT Bearer Authentication
- Swashbuckle (Swagger/OpenAPI)

### Frontend
- Angular 19
- TypeScript
- Bootstrap 5
- Bootstrap Icons
- RxJS
- Angular Router
- Angular Forms

## Development

### Running Tests
Backend:
```bash
cd backend/HotelBookingAPI
dotnet test
```

Frontend:
```bash
cd frontend
npm test
```

### Building for Production

Backend:
```bash
cd backend/HotelBookingAPI
dotnet publish -c Release
```

Frontend:
```bash
cd frontend
npm run build
```

The production build will be available in `frontend/dist/hotel-booking-app/`

## Security Notes

- The default JWT secret key in `appsettings.json` should be changed in production
- Password hashing is simplified for demo purposes; use a proper library like BCrypt in production
- Enable HTTPS in production
- Update CORS policy for production domains
- Implement rate limiting and other security measures for production deployment

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License.

## Support

For issues and questions, please open an issue on the GitHub repository.
