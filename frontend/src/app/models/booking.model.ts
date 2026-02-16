/**
 * Booking model representing a hotel booking
 */
export interface Booking {
  id: number;
  userId: number;
  roomId: number;
  checkInDate: Date;
  checkOutDate: Date;
  numberOfGuests: number;
  totalPrice: number;
  status: string;
  specialRequests?: string;
  paymentStatus: string;
  bookingDate: Date;
  updatedAt?: Date;
  confirmationCode?: string;
  user?: User;
  room?: Room;
}

/**
 * CreateBookingDto for creating a new booking
 */
export interface CreateBookingDto {
  roomId: number;
  checkInDate: Date;
  checkOutDate: Date;
  numberOfGuests: number;
  specialRequests?: string;
}

/**
 * User model (minimal, imported from hotel model for consistency)
 */
export interface User {
  id: number;
  email: string;
  fullName: string;
  phoneNumber?: string;
  role: string;
  createdAt: Date;
}

/**
 * Room model (minimal, for booking display)
 */
export interface Room {
  id: number;
  hotelId: number;
  roomNumber: string;
  roomType: string;
  pricePerNight: number;
  capacity: number;
  hotel?: {
    id: number;
    name: string;
    city: string;
    country: string;
  };
}
