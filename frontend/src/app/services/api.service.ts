import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

/**
 * Base API service for common HTTP operations
 */
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  /**
   * Base API URL from environment configuration
   */
  protected apiUrl = environment.apiUrl;

  constructor(protected http: HttpClient) {}

  /**
   * Generic GET request
   * @param endpoint API endpoint
   * @returns Observable of the response
   */
  get<T>(endpoint: string) {
    return this.http.get<T>(`${this.apiUrl}/${endpoint}`);
  }

  /**
   * Generic POST request
   * @param endpoint API endpoint
   * @param data Request body data
   * @returns Observable of the response
   */
  post<T>(endpoint: string, data: any) {
    return this.http.post<T>(`${this.apiUrl}/${endpoint}`, data);
  }

  /**
   * Generic PUT request
   * @param endpoint API endpoint
   * @param data Request body data
   * @returns Observable of the response
   */
  put<T>(endpoint: string, data: any) {
    return this.http.put<T>(`${this.apiUrl}/${endpoint}`, data);
  }

  /**
   * Generic DELETE request
   * @param endpoint API endpoint
   * @returns Observable of the response
   */
  delete<T>(endpoint: string) {
    return this.http.delete<T>(`${this.apiUrl}/${endpoint}`);
  }
}
