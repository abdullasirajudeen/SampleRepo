/**
 * Hotel model representing a hotel entity
 */
export interface Hotel {
  id: number;
  name: string;
  description: string;
  address: string;
  city: string;
  country: string;
  starRating: number;
  imageUrl?: string;
  amenities?: string;
  averageRating: number;
  reviewCount: number;
  createdAt: Date;
  isActive: boolean;
  rooms?: Room[];
  reviews?: Review[];
}

/**
 * Room model representing a hotel room
 */
export interface Room {
  id: number;
  hotelId: number;
  roomNumber: string;
  roomType: string;
  description?: string;
  pricePerNight: number;
  capacity: number;
  bedCount: number;
  sizeInSqMeters?: number;
  imageUrl?: string;
  amenities?: string;
  isAvailable: boolean;
  createdAt: Date;
  hotel?: Hotel;
}

/**
 * Review model representing a hotel review
 */
export interface Review {
  id: number;
  hotelId: number;
  userId: number;
  rating: number;
  title: string;
  comment: string;
  createdAt: Date;
  isVerified: boolean;
  helpfulCount: number;
  user?: User;
}

/**
 * User model (minimal, for review display)
 */
export interface User {
  id: number;
  fullName: string;
  email: string;
}
