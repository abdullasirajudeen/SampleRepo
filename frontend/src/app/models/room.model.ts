export interface Room {
  id: number;
  hotelId: number;
  hotelName: string;
  roomNumber: string;
  roomType: string;
  pricePerNight: number;
  maxOccupancy: number;
  description?: string;
  imageUrl?: string;
  hasWifi: boolean;
  hasAirConditioning: boolean;
  hasTV: boolean;
  hasMiniBar: boolean;
  isAvailable: boolean;
}

export interface CreateRoom {
  hotelId: number;
  roomNumber: string;
  roomType: string;
  pricePerNight: number;
  maxOccupancy: number;
  description?: string;
  imageUrl?: string;
  hasWifi: boolean;
  hasAirConditioning: boolean;
  hasTV: boolean;
  hasMiniBar: boolean;
}

export interface UpdateRoom {
  roomNumber: string;
  roomType: string;
  pricePerNight: number;
  maxOccupancy: number;
  description?: string;
  imageUrl?: string;
  hasWifi: boolean;
  hasAirConditioning: boolean;
  hasTV: boolean;
  hasMiniBar: boolean;
  isAvailable: boolean;
}
