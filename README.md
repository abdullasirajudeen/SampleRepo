# Hotel Booking System

A complete hotel booking web application built with .NET 8 backend and Angular frontend.

## Features

### Backend (.NET 8)
- RESTful API with full CRUD operations
- Entity Framework Core with SQLite database
- Clean architecture with services, repositories, and DTOs
- Swagger/OpenAPI documentation
- CORS configuration for Angular frontend
- Comprehensive error handling
- Data seeding for demo purposes

### Frontend (Angular)
- Modern, responsive UI design
- Hotel search and filtering
- Hotel details with room availability
- Booking management
- User authentication (login/register)
- HTTP interceptor for error handling
- Reactive forms and services
- Mobile-responsive design

## Technology Stack

### Backend
- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core 8.0
- SQLite Database
- Swashbuckle (Swagger)

### Frontend
- Angular 17
- TypeScript
- RxJS
- HTML5/CSS3
- HttpClient for API communication

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or later)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)

## Getting Started

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd backend/HotelBooking.API
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. The API will start on `http://localhost:5000` and `https://localhost:5001`

6. Access Swagger UI at: `http://localhost:5000` (redirects to Swagger)

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install npm packages:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```
   or
   ```bash
   ng serve
   ```

4. The application will start on `http://localhost:4200`

5. Open your browser and navigate to `http://localhost:4200`

## Project Structure

### Backend Structure
```
backend/HotelBooking.API/
├── Controllers/          # API controllers
│   ├── HotelsController.cs
│   ├── RoomsController.cs
│   ├── BookingsController.cs
│   ├── UsersController.cs
│   └── ReviewsController.cs
├── Models/              # Entity models
│   ├── User.cs
│   ├── Hotel.cs
│   ├── Room.cs
│   ├── Booking.cs
│   └── Review.cs
├── DTOs/               # Data transfer objects
│   ├── CreateHotelDto.cs
│   ├── CreateBookingDto.cs
│   └── UserDto.cs
├── Services/           # Business logic services
│   ├── IHotelService.cs
│   ├── HotelService.cs
│   ├── IBookingService.cs
│   └── BookingService.cs
├── Data/              # Database context and initializer
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
├── Program.cs         # Application entry point
└── appsettings.json   # Configuration
```

### Frontend Structure
```
frontend/src/
├── app/
│   ├── components/           # Angular components
│   │   ├── home/
│   │   ├── navbar/
│   │   ├── hotel-list/
│   │   ├── hotel-details/
│   │   └── booking-form/
│   ├── models/              # TypeScript interfaces
│   │   ├── hotel.model.ts
│   │   ├── booking.model.ts
│   │   └── user.model.ts
│   ├── services/            # Angular services
│   │   ├── api.service.ts
│   │   ├── hotel.service.ts
│   │   ├── booking.service.ts
│   │   └── auth.service.ts
│   ├── interceptors/        # HTTP interceptors
│   │   └── http.interceptor.ts
│   ├── app.module.ts        # Main module
│   └── app-routing.module.ts # Routing configuration
├── environments/            # Environment configs
│   ├── environment.ts
│   └── environment.prod.ts
├── assets/                 # Static assets
└── styles.css             # Global styles
```

## API Endpoints

### Hotels
- `GET /api/hotels` - Get all hotels
- `GET /api/hotels/{id}` - Get hotel by ID
- `GET /api/hotels/search` - Search hotels (query params: city, minRating, maxPrice)
- `POST /api/hotels` - Create hotel (Admin)
- `PUT /api/hotels/{id}` - Update hotel (Admin)
- `DELETE /api/hotels/{id}` - Delete hotel (Admin)
- `GET /api/hotels/{id}/rooms` - Get hotel rooms
- `GET /api/hotels/{id}/available-rooms` - Get available rooms

### Bookings
- `GET /api/bookings/user/{userId}` - Get user bookings
- `GET /api/bookings/{id}` - Get booking by ID
- `POST /api/bookings/user/{userId}` - Create booking
- `DELETE /api/bookings/{id}/user/{userId}` - Cancel booking
- `GET /api/bookings/check-availability` - Check room availability
- `PUT /api/bookings/{id}/confirm` - Confirm booking

### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users/register` - Register new user
- `POST /api/users/login` - User login
- `PUT /api/users/{id}` - Update user profile

### Rooms
- `GET /api/rooms` - Get all rooms
- `GET /api/rooms/{id}` - Get room by ID
- `POST /api/rooms` - Create room (Admin)
- `PUT /api/rooms/{id}` - Update room (Admin)
- `DELETE /api/rooms/{id}` - Delete room (Admin)

### Reviews
- `GET /api/reviews/hotel/{hotelId}` - Get hotel reviews
- `GET /api/reviews/{id}` - Get review by ID
- `POST /api/reviews` - Create review
- `PUT /api/reviews/{id}` - Update review
- `DELETE /api/reviews/{id}` - Delete review
- `PUT /api/reviews/{id}/helpful` - Mark review as helpful

## Database

The application uses SQLite database for simplicity. The database file (`hotelbooking.db`) is automatically created when you run the application for the first time.

### Sample Data

The application seeds the database with sample data:
- 3 hotels (Grand Plaza Hotel, Seaside Resort, Mountain View Lodge)
- 9 rooms (3 per hotel with different types)
- 3 users (Admin, John Doe, Jane Smith)
- 3 reviews

### Database Schema

The database includes the following tables:
- **Users** - User accounts and authentication
- **Hotels** - Hotel information
- **Rooms** - Room details and pricing
- **Bookings** - Booking records
- **Reviews** - Customer reviews

## Environment Configuration

### Backend
Edit `appsettings.json` to configure:
- Connection string (database location)
- Logging levels
- Application settings

### Frontend
Edit `src/environments/environment.ts` for development and `environment.prod.ts` for production:
- API URL
- Production flag

## Building for Production

### Backend
```bash
cd backend/HotelBooking.API
dotnet publish -c Release -o ./publish
```

### Frontend
```bash
cd frontend
ng build --configuration production
```

The production files will be in `frontend/dist/hotel-booking-frontend/`

## Docker Support (Optional)

A `docker-compose.yml` file is provided for containerized deployment:

```bash
docker-compose up --build
```

This will start both the backend and frontend services.

## Development Notes

### Backend
- The backend uses a simple SHA256 password hashing for demonstration. In production, use a proper password hashing library like BCrypt.Net or AspNetCore.Identity.
- JWT authentication can be added for more secure API access.
- The SQLite database can be replaced with SQL Server, PostgreSQL, or other databases by changing the connection string and provider.

### Frontend
- The application uses localStorage for maintaining user sessions. For production, implement proper JWT token management.
- Add form validation and better error handling for production use.
- Implement proper authentication guards for protected routes.

## Testing

### Backend
```bash
cd backend/HotelBooking.API
dotnet test
```

### Frontend
```bash
cd frontend
ng test
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License.

## Support

For issues, questions, or contributions, please create an issue in the repository.

## Future Enhancements

- Payment gateway integration
- Email notifications
- Advanced search filters
- Hotel admin dashboard
- Multi-language support
- Social media authentication
- Booking history and receipts
- Loyalty program
- Mobile application
- Real-time availability updates

## Acknowledgments

- Built with modern web technologies
- Designed for scalability and maintainability
- Production-ready with best practices
