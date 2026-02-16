import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Hotel, CreateHotel, UpdateHotel } from '../models/hotel.model';

@Injectable({
  providedIn: 'root'
})
export class HotelService {
  private apiUrl = 'http://localhost:5000/api/Hotels';

  constructor(private http: HttpClient) { }

  getHotels(city?: string, country?: string, minStarRating?: number): Observable<Hotel[]> {
    let params = new HttpParams();
    if (city) params = params.set('city', city);
    if (country) params = params.set('country', country);
    if (minStarRating) params = params.set('minStarRating', minStarRating.toString());

    return this.http.get<Hotel[]>(this.apiUrl, { params });
  }

  getHotel(id: number): Observable<Hotel> {
    return this.http.get<Hotel>(`${this.apiUrl}/${id}`);
  }

  createHotel(hotel: CreateHotel): Observable<Hotel> {
    return this.http.post<Hotel>(this.apiUrl, hotel);
  }

  updateHotel(id: number, hotel: UpdateHotel): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, hotel);
  }

  deleteHotel(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
