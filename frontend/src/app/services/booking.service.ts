import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Booking, CreateBooking, UpdateBooking } from '../models/booking.model';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private apiUrl = 'http://localhost:5000/api/Bookings';

  constructor(private http: HttpClient) { }

  getBookings(userId?: number): Observable<Booking[]> {
    let params = new HttpParams();
    if (userId) params = params.set('userId', userId.toString());

    return this.http.get<Booking[]>(this.apiUrl, { params });
  }

  getBooking(id: number): Observable<Booking> {
    return this.http.get<Booking>(`${this.apiUrl}/${id}`);
  }

  createBooking(booking: CreateBooking, userId: number): Observable<Booking> {
    return this.http.post<Booking>(`${this.apiUrl}?userId=${userId}`, booking);
  }

  updateBooking(id: number, booking: UpdateBooking): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, booking);
  }

  cancelBooking(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/cancel`, {});
  }

  confirmBooking(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/confirm`, {});
  }
}
