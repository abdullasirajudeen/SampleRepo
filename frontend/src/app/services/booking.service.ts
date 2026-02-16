import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Booking, CreateBookingDto } from '../models/booking.model';

/**
 * Service for booking-related API operations
 */
@Injectable({
  providedIn: 'root'
})
export class BookingService extends ApiService {
  /**
   * Gets all bookings for a specific user
   * @param userId User ID
   * @returns Observable of booking array
   */
  getUserBookings(userId: number): Observable<Booking[]> {
    return this.get<Booking[]>(`bookings/user/${userId}`);
  }

  /**
   * Gets a specific booking by ID
   * @param id Booking ID
   * @returns Observable of booking
   */
  getBookingById(id: number): Observable<Booking> {
    return this.get<Booking>(`bookings/${id}`);
  }

  /**
   * Creates a new booking
   * @param userId User ID making the booking
   * @param bookingData Booking creation data
   * @returns Observable of created booking
   */
  createBooking(userId: number, bookingData: CreateBookingDto): Observable<Booking> {
    return this.post<Booking>(`bookings/user/${userId}`, bookingData);
  }

  /**
   * Cancels a booking
   * @param id Booking ID
   * @param userId User ID requesting cancellation
   * @returns Observable of response
   */
  cancelBooking(id: number, userId: number): Observable<any> {
    return this.delete<any>(`bookings/${id}/user/${userId}`);
  }

  /**
   * Checks room availability
   * @param roomId Room ID
   * @param checkIn Check-in date
   * @param checkOut Check-out date
   * @returns Observable of availability status
   */
  checkAvailability(roomId: number, checkIn: Date, checkOut: Date): Observable<any> {
    const checkInStr = checkIn.toISOString().split('T')[0];
    const checkOutStr = checkOut.toISOString().split('T')[0];
    return this.get<any>(`bookings/check-availability?roomId=${roomId}&checkIn=${checkInStr}&checkOut=${checkOutStr}`);
  }

  /**
   * Confirms a booking
   * @param id Booking ID
   * @returns Observable of response
   */
  confirmBooking(id: number): Observable<any> {
    return this.put<any>(`bookings/${id}/confirm`, {});
  }

  /**
   * Calculates total price for a booking
   * @param pricePerNight Price per night
   * @param checkIn Check-in date
   * @param checkOut Check-out date
   * @returns Total price
   */
  calculateTotalPrice(pricePerNight: number, checkIn: Date, checkOut: Date): number {
    const nights = Math.ceil((checkOut.getTime() - checkIn.getTime()) / (1000 * 60 * 60 * 24));
    return pricePerNight * nights;
  }
}
