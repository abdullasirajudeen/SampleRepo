import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingService } from '../../services/booking.service';
import { AuthService } from '../../services/auth.service';
import { HotelService } from '../../services/hotel.service';
import { Room } from '../../models/hotel.model';
import { CreateBookingDto } from '../../models/booking.model';

/**
 * Booking form component for creating hotel room bookings
 */
@Component({
  selector: 'app-booking-form',
  templateUrl: './booking-form.component.html',
  styleUrls: ['./booking-form.component.css']
})
export class BookingFormComponent implements OnInit {
  /**
   * Room to book
   */
  room: Room | null = null;

  /**
   * Booking form data
   */
  bookingData: CreateBookingDto = {
    roomId: 0,
    checkInDate: new Date(),
    checkOutDate: new Date(),
    numberOfGuests: 1,
    specialRequests: ''
  };

  /**
   * Total price calculation
   */
  totalPrice: number = 0;

  /**
   * Number of nights
   */
  numberOfNights: number = 0;

  /**
   * Loading state
   */
  isLoading = false;

  /**
   * Success message
   */
  successMessage: string = '';

  /**
   * Error message
   */
  errorMessage: string = '';

  constructor(
    private bookingService: BookingService,
    private authService: AuthService,
    private hotelService: HotelService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Check if user is authenticated
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login'], {
        queryParams: { returnUrl: this.router.url }
      });
      return;
    }

    // Get booking parameters from query string
    this.route.queryParams.subscribe(params => {
      const roomId = parseInt(params['roomId']);
      const checkIn = params['checkIn'];
      const checkOut = params['checkOut'];

      if (roomId) {
        this.bookingData.roomId = roomId;
        this.loadRoomDetails(roomId);
      }

      if (checkIn) {
        this.bookingData.checkInDate = new Date(checkIn);
      }

      if (checkOut) {
        this.bookingData.checkOutDate = new Date(checkOut);
      }

      this.calculatePrice();
    });
  }

  /**
   * Loads room details
   */
  loadRoomDetails(roomId: number): void {
    // In a real app, we'd have a RoomService to get room details
    // For now, we'll use a simplified approach
  }

  /**
   * Calculates total price and number of nights
   */
  calculatePrice(): void {
    if (this.room && this.bookingData.checkInDate && this.bookingData.checkOutDate) {
      this.totalPrice = this.bookingService.calculateTotalPrice(
        this.room.pricePerNight,
        this.bookingData.checkInDate,
        this.bookingData.checkOutDate
      );

      this.numberOfNights = Math.ceil(
        (this.bookingData.checkOutDate.getTime() - this.bookingData.checkInDate.getTime()) 
        / (1000 * 60 * 60 * 24)
      );
    }
  }

  /**
   * Validates the booking form
   */
  validateBooking(): boolean {
    if (!this.bookingData.checkInDate || !this.bookingData.checkOutDate) {
      this.errorMessage = 'Please select check-in and check-out dates';
      return false;
    }

    if (this.bookingData.checkOutDate <= this.bookingData.checkInDate) {
      this.errorMessage = 'Check-out date must be after check-in date';
      return false;
    }

    if (this.bookingData.numberOfGuests < 1) {
      this.errorMessage = 'Number of guests must be at least 1';
      return false;
    }

    if (this.room && this.bookingData.numberOfGuests > this.room.capacity) {
      this.errorMessage = `This room can accommodate maximum ${this.room.capacity} guests`;
      return false;
    }

    return true;
  }

  /**
   * Submits the booking
   */
  submitBooking(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.validateBooking()) {
      return;
    }

    const currentUser = this.authService.getCurrentUser();
    if (!currentUser) {
      this.router.navigate(['/login']);
      return;
    }

    this.isLoading = true;

    this.bookingService.createBooking(currentUser.id, this.bookingData).subscribe({
      next: (booking) => {
        this.isLoading = false;
        this.successMessage = `Booking confirmed! Confirmation code: ${booking.confirmationCode}`;
        
        // Redirect to bookings page after 3 seconds
        setTimeout(() => {
          this.router.navigate(['/bookings']);
        }, 3000);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error || 'Failed to create booking. Please try again.';
        console.error('Error creating booking:', error);
      }
    });
  }

  /**
   * Cancels and returns to hotel details
   */
  cancel(): void {
    if (this.room?.hotel) {
      this.router.navigate(['/hotels', this.room.hotel.id]);
    } else {
      this.router.navigate(['/hotels']);
    }
  }

  /**
   * Updates price when dates change
   */
  onDateChange(): void {
    this.calculatePrice();
  }
}
