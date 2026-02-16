import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HotelService } from '../../services/hotel.service';
import { Hotel } from '../../models/hotel.model';

/**
 * Hotel list component for displaying and searching hotels
 */
@Component({
  selector: 'app-hotel-list',
  templateUrl: './hotel-list.component.html',
  styleUrls: ['./hotel-list.component.css']
})
export class HotelListComponent implements OnInit {
  /**
   * List of hotels to display
   */
  hotels: Hotel[] = [];

  /**
   * Search filters
   */
  searchCity: string = '';
  minRating: number | null = null;
  maxPrice: number | null = null;

  /**
   * Loading state
   */
  isLoading = false;

  /**
   * Error message
   */
  errorMessage: string = '';

  constructor(
    private hotelService: HotelService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Get search parameters from query string
    this.route.queryParams.subscribe(params => {
      this.searchCity = params['city'] || '';
      this.minRating = params['minRating'] ? parseInt(params['minRating']) : null;
      this.maxPrice = params['maxPrice'] ? parseFloat(params['maxPrice']) : null;

      // Load hotels based on search criteria
      this.loadHotels();
    });
  }

  /**
   * Loads hotels from the API
   */
  loadHotels(): void {
    this.isLoading = true;
    this.errorMessage = '';

    // If search criteria are provided, use search endpoint
    if (this.searchCity || this.minRating || this.maxPrice) {
      this.hotelService.searchHotels(this.searchCity, this.minRating || undefined, this.maxPrice || undefined)
        .subscribe({
          next: (hotels) => {
            this.hotels = hotels;
            this.isLoading = false;
          },
          error: (error) => {
            this.errorMessage = 'Failed to load hotels. Please try again.';
            this.isLoading = false;
            console.error('Error loading hotels:', error);
          }
        });
    } else {
      // Otherwise, get all hotels
      this.hotelService.getAllHotels()
        .subscribe({
          next: (hotels) => {
            this.hotels = hotels;
            this.isLoading = false;
          },
          error: (error) => {
            this.errorMessage = 'Failed to load hotels. Please try again.';
            this.isLoading = false;
            console.error('Error loading hotels:', error);
          }
        });
    }
  }

  /**
   * Applies search filters
   */
  applyFilters(): void {
    this.router.navigate(['/hotels'], {
      queryParams: {
        city: this.searchCity || undefined,
        minRating: this.minRating || undefined,
        maxPrice: this.maxPrice || undefined
      }
    });
  }

  /**
   * Clears all filters
   */
  clearFilters(): void {
    this.searchCity = '';
    this.minRating = null;
    this.maxPrice = null;
    this.router.navigate(['/hotels']);
  }

  /**
   * Navigates to hotel details page
   */
  viewHotelDetails(hotelId: number): void {
    this.router.navigate(['/hotels', hotelId]);
  }

  /**
   * Generates star rating array
   */
  getStarArray(rating: number): number[] {
    return Array(rating).fill(0);
  }

  /**
   * Gets minimum price from hotel rooms
   */
  getMinPrice(hotel: Hotel): number {
    if (!hotel.rooms || hotel.rooms.length === 0) {
      return 0;
    }
    return Math.min(...hotel.rooms.map(r => r.pricePerNight));
  }
}
