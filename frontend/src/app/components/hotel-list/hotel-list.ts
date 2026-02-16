import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HotelService } from '../../services/hotel.service';
import { Hotel } from '../../models/hotel.model';

@Component({
  selector: 'app-hotel-list',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './hotel-list.html',
  styleUrl: './hotel-list.css',
})
export class HotelList implements OnInit {
  hotels: Hotel[] = [];
  filteredHotels: Hotel[] = [];
  isLoading = true;
  errorMessage = '';
  
  // Filters
  searchCity = '';
  searchCountry = '';
  minStarRating: number | undefined;

  constructor(private hotelService: HotelService) {}

  ngOnInit(): void {
    this.loadHotels();
  }

  loadHotels(): void {
    this.isLoading = true;
    this.hotelService.getHotels(this.searchCity, this.searchCountry, this.minStarRating).subscribe({
      next: (data) => {
        this.hotels = data;
        this.filteredHotels = data;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Failed to load hotels';
        this.isLoading = false;
      }
    });
  }

  applyFilters(): void {
    this.loadHotels();
  }

  clearFilters(): void {
    this.searchCity = '';
    this.searchCountry = '';
    this.minStarRating = undefined;
    this.loadHotels();
  }

  getStarArray(rating: number): number[] {
    return Array(rating).fill(0);
  }
}
