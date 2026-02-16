import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

/**
 * Home component - Landing page for the hotel booking application
 */
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  /**
   * Search criteria
   */
  searchCity: string = '';
  checkInDate: Date | null = null;
  checkOutDate: Date | null = null;
  guests: number = 1;

  constructor(private router: Router) {}

  ngOnInit(): void {
    // Set default dates (tomorrow and day after for better user experience)
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);
    const dayAfter = new Date(tomorrow);
    dayAfter.setDate(dayAfter.getDate() + 1);
    
    this.checkInDate = tomorrow;
    this.checkOutDate = dayAfter;
  }

  /**
   * Performs hotel search based on criteria
   */
  searchHotels(): void {
    // Navigate to hotel list with search parameters
    this.router.navigate(['/hotels'], {
      queryParams: {
        city: this.searchCity || undefined,
        checkIn: this.checkInDate?.toISOString() || undefined,
        checkOut: this.checkOutDate?.toISOString() || undefined,
        guests: this.guests
      }
    });
  }

  /**
   * Navigates to all hotels page
   */
  viewAllHotels(): void {
    this.router.navigate(['/hotels']);
  }
}
