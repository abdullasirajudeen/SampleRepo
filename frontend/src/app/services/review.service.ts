import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Review, CreateReview, UpdateReview } from '../models/review.model';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private apiUrl = 'http://localhost:5000/api/Reviews';

  constructor(private http: HttpClient) { }

  getReviews(hotelId?: number, userId?: number): Observable<Review[]> {
    let params = new HttpParams();
    if (hotelId) params = params.set('hotelId', hotelId.toString());
    if (userId) params = params.set('userId', userId.toString());

    return this.http.get<Review[]>(this.apiUrl, { params });
  }

  getReview(id: number): Observable<Review> {
    return this.http.get<Review>(`${this.apiUrl}/${id}`);
  }

  createReview(review: CreateReview, userId: number): Observable<Review> {
    return this.http.post<Review>(`${this.apiUrl}?userId=${userId}`, review);
  }

  updateReview(id: number, review: UpdateReview, userId: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}?userId=${userId}`, review);
  }

  deleteReview(id: number, userId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}?userId=${userId}`);
  }

  approveReview(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/approve`, {});
  }
}
