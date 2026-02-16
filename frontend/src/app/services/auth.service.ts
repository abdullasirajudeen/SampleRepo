import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import { User, LoginDto, RegisterUserDto } from '../models/user.model';

/**
 * Service for authentication and user management
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService extends ApiService {
  /**
   * Current user subject for reactive updates
   */
  private currentUserSubject = new BehaviorSubject<User | null>(null);

  /**
   * Observable for current user
   */
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor() {
    super(null as any); // Will be injected properly in module
    this.loadUserFromStorage();
  }

  /**
   * Registers a new user
   * @param registerData User registration data
   * @returns Observable of created user
   */
  register(registerData: RegisterUserDto): Observable<User> {
    return this.post<User>('users/register', registerData).pipe(
      tap(user => this.setCurrentUser(user))
    );
  }

  /**
   * Authenticates a user
   * @param loginData Login credentials
   * @returns Observable of user
   */
  login(loginData: LoginDto): Observable<User> {
    return this.post<User>('users/login', loginData).pipe(
      tap(user => this.setCurrentUser(user))
    );
  }

  /**
   * Logs out the current user
   */
  logout(): void {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  /**
   * Gets the current user
   * @returns Current user or null
   */
  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  /**
   * Checks if user is authenticated
   * @returns True if user is logged in
   */
  isAuthenticated(): boolean {
    return this.currentUserSubject.value !== null;
  }

  /**
   * Checks if current user is admin
   * @returns True if user has admin role
   */
  isAdmin(): boolean {
    const user = this.currentUserSubject.value;
    return user !== null && user.role === 'Admin';
  }

  /**
   * Gets user by ID
   * @param id User ID
   * @returns Observable of user
   */
  getUserById(id: number): Observable<User> {
    return this.get<User>(`users/${id}`);
  }

  /**
   * Updates user profile
   * @param id User ID
   * @param userData Updated user data
   * @returns Observable of response
   */
  updateUser(id: number, userData: any): Observable<any> {
    return this.put<any>(`users/${id}`, userData);
  }

  /**
   * Sets the current user and saves to local storage
   * @param user User to set as current
   */
  private setCurrentUser(user: User): void {
    localStorage.setItem('currentUser', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  /**
   * Loads user from local storage on app initialization
   */
  private loadUserFromStorage(): void {
    const userJson = localStorage.getItem('currentUser');
    if (userJson) {
      try {
        const user = JSON.parse(userJson);
        this.currentUserSubject.next(user);
      } catch (error) {
        console.error('Error loading user from storage:', error);
        localStorage.removeItem('currentUser');
      }
    }
  }
}
