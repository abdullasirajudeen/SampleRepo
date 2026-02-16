import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { BookingService } from '../../services/booking.service';
import { User } from '../../models/user.model';
import { Booking } from '../../models/booking.model';

@Component({
  selector: 'app-user-profile',
  imports: [CommonModule],
  templateUrl: './user-profile.html',
  styleUrl: './user-profile.css',
})
export class UserProfile implements OnInit {
  user: User | null = null;
  bookings: Booking[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private bookingService: BookingService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.user = this.authService.getCurrentUser();
    if (this.user) {
      this.loadBookings(this.user.id);
    }
  }

  loadBookings(userId: number): void {
    this.isLoading = true;
    this.bookingService.getBookings(userId).subscribe({
      next: (data) => {
        this.bookings = data;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Failed to load bookings';
        this.isLoading = false;
      }
    });
  }

  cancelBooking(bookingId: number): void {
    if (confirm('Are you sure you want to cancel this booking?')) {
      this.bookingService.cancelBooking(bookingId).subscribe({
        next: () => {
          alert('Booking cancelled successfully');
          if (this.user) {
            this.loadBookings(this.user.id);
          }
        },
        error: (error) => {
          alert(error.message || 'Failed to cancel booking');
        }
      });
    }
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'confirmed':
        return 'badge bg-success';
      case 'pending':
        return 'badge bg-warning';
      case 'cancelled':
        return 'badge bg-danger';
      case 'completed':
        return 'badge bg-info';
      default:
        return 'badge bg-secondary';
    }
  }
}
