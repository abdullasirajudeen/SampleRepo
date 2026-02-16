import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RoomService } from '../../services/room.service';
import { BookingService } from '../../services/booking.service';
import { AuthService } from '../../services/auth.service';
import { Room } from '../../models/room.model';
import { CreateBooking } from '../../models/booking.model';

@Component({
  selector: 'app-booking-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './booking-form.html',
  styleUrl: './booking-form.css',
})
export class BookingForm implements OnInit {
  room: Room | null = null;
  booking: CreateBooking = {
    roomId: 0,
    checkInDate: new Date(),
    checkOutDate: new Date(),
    numberOfGuests: 1,
    specialRequests: ''
  };
  isLoading = false;
  errorMessage = '';
  totalPrice = 0;
  numberOfNights = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private roomService: RoomService,
    private bookingService: BookingService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    const roomId = this.route.snapshot.params['roomId'];
    if (roomId) {
      this.booking.roomId = +roomId;
      this.loadRoom(+roomId);
    }

    // Set default dates
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);
    
    this.booking.checkInDate = today;
    this.booking.checkOutDate = tomorrow;
    this.calculatePrice();
  }

  loadRoom(roomId: number): void {
    this.roomService.getRoom(roomId).subscribe({
      next: (data) => {
        this.room = data;
        this.calculatePrice();
      },
      error: (error) => {
        this.errorMessage = error.message || 'Failed to load room details';
      }
    });
  }

  calculatePrice(): void {
    if (!this.room) return;
    
    const checkIn = new Date(this.booking.checkInDate);
    const checkOut = new Date(this.booking.checkOutDate);
    const timeDiff = checkOut.getTime() - checkIn.getTime();
    this.numberOfNights = Math.ceil(timeDiff / (1000 * 3600 * 24));
    
    if (this.numberOfNights > 0) {
      this.totalPrice = this.room.pricePerNight * this.numberOfNights;
    } else {
      this.totalPrice = 0;
    }
  }

  onSubmit(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    const user = this.authService.getCurrentUser();
    if (!user) {
      this.errorMessage = 'User not found';
      return;
    }

    this.errorMessage = '';
    this.isLoading = true;

    this.bookingService.createBooking(this.booking, user.id).subscribe({
      next: () => {
        alert('Booking created successfully!');
        this.router.navigate(['/profile']);
      },
      error: (error) => {
        this.errorMessage = error.message || 'Failed to create booking';
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }
}
