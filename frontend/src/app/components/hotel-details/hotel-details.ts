import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { HotelService } from '../../services/hotel.service';
import { RoomService } from '../../services/room.service';
import { ReviewService } from '../../services/review.service';
import { Hotel } from '../../models/hotel.model';
import { Room } from '../../models/room.model';
import { Review } from '../../models/review.model';

@Component({
  selector: 'app-hotel-details',
  imports: [CommonModule, RouterModule],
  templateUrl: './hotel-details.html',
  styleUrl: './hotel-details.css',
})
export class HotelDetails implements OnInit {
  hotel: Hotel | null = null;
  rooms: Room[] = [];
  reviews: Review[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private hotelService: HotelService,
    private roomService: RoomService,
    private reviewService: ReviewService
  ) {}

  ngOnInit(): void {
    const hotelId = this.route.snapshot.params['id'];
    if (hotelId) {
      this.loadHotelDetails(+hotelId);
    }
  }

  loadHotelDetails(hotelId: number): void {
    this.isLoading = true;
    
    this.hotelService.getHotel(hotelId).subscribe({
      next: (data) => {
        this.hotel = data;
        this.loadRooms(hotelId);
        this.loadReviews(hotelId);
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message || 'Failed to load hotel details';
        this.isLoading = false;
      }
    });
  }

  loadRooms(hotelId: number): void {
    this.roomService.getRooms(hotelId).subscribe({
      next: (data) => {
        this.rooms = data;
      },
      error: (error) => {
        console.error('Failed to load rooms:', error);
      }
    });
  }

  loadReviews(hotelId: number): void {
    this.reviewService.getReviews(hotelId).subscribe({
      next: (data) => {
        this.reviews = data;
      },
      error: (error) => {
        console.error('Failed to load reviews:', error);
      }
    });
  }

  getStarArray(rating: number): number[] {
    return Array(rating).fill(0);
  }
}
