import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Room, CreateRoom, UpdateRoom } from '../models/room.model';

@Injectable({
  providedIn: 'root'
})
export class RoomService {
  private apiUrl = 'http://localhost:5000/api/Rooms';

  constructor(private http: HttpClient) { }

  getRooms(hotelId?: number): Observable<Room[]> {
    let params = new HttpParams();
    if (hotelId) params = params.set('hotelId', hotelId.toString());

    return this.http.get<Room[]>(this.apiUrl, { params });
  }

  getRoom(id: number): Observable<Room> {
    return this.http.get<Room>(`${this.apiUrl}/${id}`);
  }

  checkAvailability(roomId: number, checkInDate: Date, checkOutDate: Date): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/CheckAvailability`, {
      roomId,
      checkInDate,
      checkOutDate
    });
  }

  createRoom(room: CreateRoom): Observable<Room> {
    return this.http.post<Room>(this.apiUrl, room);
  }

  updateRoom(id: number, room: UpdateRoom): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, room);
  }

  deleteRoom(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
