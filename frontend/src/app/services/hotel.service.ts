import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Hotel, Room } from '../models/hotel.model';

/**
 * Service for hotel-related API operations
 */
@Injectable({
  providedIn: 'root'
})
export class HotelService extends ApiService {
  /**
   * Gets all hotels
   * @returns Observable of hotel array
   */
  getAllHotels(): Observable<Hotel[]> {
    return this.get<Hotel[]>('hotels');
  }

  /**
   * Gets a specific hotel by ID
   * @param id Hotel ID
   * @returns Observable of hotel
   */
  getHotelById(id: number): Observable<Hotel> {
    return this.get<Hotel>(`hotels/${id}`);
  }

  /**
   * Searches hotels based on criteria
   * @param city City to search in
   * @param minRating Minimum star rating
   * @param maxPrice Maximum price per night
   * @returns Observable of hotel array
   */
  searchHotels(city?: string, minRating?: number, maxPrice?: number): Observable<Hotel[]> {
    let query = 'hotels/search?';
    const params: string[] = [];

    if (city) {
      params.push(`city=${encodeURIComponent(city)}`);
    }
    if (minRating) {
      params.push(`minRating=${minRating}`);
    }
    if (maxPrice) {
      params.push(`maxPrice=${maxPrice}`);
    }

    query += params.join('&');
    return this.get<Hotel[]>(query);
  }

  /**
   * Creates a new hotel (admin only)
   * @param hotel Hotel data
   * @returns Observable of created hotel
   */
  createHotel(hotel: any): Observable<Hotel> {
    return this.post<Hotel>('hotels', hotel);
  }

  /**
   * Updates an existing hotel (admin only)
   * @param id Hotel ID
   * @param hotel Updated hotel data
   * @returns Observable of response
   */
  updateHotel(id: number, hotel: any): Observable<any> {
    return this.put<any>(`hotels/${id}`, hotel);
  }

  /**
   * Deletes a hotel (admin only)
   * @param id Hotel ID
   * @returns Observable of response
   */
  deleteHotel(id: number): Observable<any> {
    return this.delete<any>(`hotels/${id}`);
  }

  /**
   * Gets all rooms for a specific hotel
   * @param hotelId Hotel ID
   * @returns Observable of room array
   */
  getHotelRooms(hotelId: number): Observable<Room[]> {
    return this.get<Room[]>(`hotels/${hotelId}/rooms`);
  }

  /**
   * Gets available rooms for a hotel in a date range
   * @param hotelId Hotel ID
   * @param checkIn Check-in date
   * @param checkOut Check-out date
   * @returns Observable of room array
   */
  getAvailableRooms(hotelId: number, checkIn: Date, checkOut: Date): Observable<Room[]> {
    const checkInStr = checkIn.toISOString().split('T')[0];
    const checkOutStr = checkOut.toISOString().split('T')[0];
    return this.get<Room[]>(`hotels/${hotelId}/available-rooms?checkIn=${checkInStr}&checkOut=${checkOutStr}`);
  }
}
