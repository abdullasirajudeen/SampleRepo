import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HotelService } from '../../services/hotel.service';
import { Hotel, Room } from '../../models/hotel.model';

/**
 * Hotel details component for displaying detailed hotel information
 */
@Component({
  selector: 'app-hotel-details',
  templateUrl: './hotel-details.component.html',
  styleUrls: ['./hotel-details.component.css']
})
export class HotelDetailsComponent implements OnInit {
  /**
   * Hotel to display
   */
  hotel: Hotel | null = null;

  /**
   * Available rooms
   */
  rooms: Room[] = [];

  /**
   * Selected dates for room search
   */
  checkInDate: Date | null = null;
  checkOutDate: Date | null = null;

  /**
   * Loading state
   */
  isLoading = false;

  /**
   * Error message
   */
  errorMessage: string = '';

  /**
   * Active tab
   */
  activeTab: 'overview' | 'rooms' | 'reviews' = 'overview';

  constructor(
    private hotelService: HotelService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Get hotel ID from route parameters
    this.route.params.subscribe(params => {
      const hotelId = parseInt(params['id']);
      if (hotelId) {
        this.loadHotelDetails(hotelId);
        this.loadHotelRooms(hotelId);
      }
    });

    // Set default dates
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);
    this.checkInDate = today;
    this.checkOutDate = tomorrow;
  }

  /**
   * Loads hotel details from the API
   */
  loadHotelDetails(hotelId: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.hotelService.getHotelById(hotelId).subscribe({
      next: (hotel) => {
        this.hotel = hotel;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Failed to load hotel details. Please try again.';
        this.isLoading = false;
        console.error('Error loading hotel details:', error);
      }
    });
  }

  /**
   * Loads hotel rooms
   */
  loadHotelRooms(hotelId: number): void {
    this.hotelService.getHotelRooms(hotelId).subscribe({
      next: (rooms) => {
        this.rooms = rooms;
      },
      error: (error) => {
        console.error('Error loading hotel rooms:', error);
      }
    });
  }

  /**
   * Searches for available rooms based on dates
   */
  searchAvailableRooms(): void {
    if (!this.hotel || !this.checkInDate || !this.checkOutDate) {
      return;
    }

    this.hotelService.getAvailableRooms(
      this.hotel.id,
      this.checkInDate,
      this.checkOutDate
    ).subscribe({
      next: (rooms) => {
        this.rooms = rooms;
      },
      error: (error) => {
        console.error('Error loading available rooms:', error);
      }
    });
  }

  /**
   * Navigates to booking form for a specific room
   */
  bookRoom(room: Room): void {
    if (!this.checkInDate || !this.checkOutDate) {
      alert('Please select check-in and check-out dates');
      return;
    }

    this.router.navigate(['/booking'], {
      queryParams: {
        roomId: room.id,
        checkIn: this.checkInDate.toISOString(),
        checkOut: this.checkOutDate.toISOString()
      }
    });
  }

  /**
   * Changes active tab
   */
  setActiveTab(tab: 'overview' | 'rooms' | 'reviews'): void {
    this.activeTab = tab;
  }

  /**
   * Generates star rating array
   */
  getStarArray(rating: number): number[] {
    return Array(rating).fill(0);
  }

  /**
   * Goes back to hotel list
   */
  goBack(): void {
    this.router.navigate(['/hotels']);
  }
}
