export interface Booking {
  id: number;
  userId: number;
  userName: string;
  roomId: number;
  roomNumber: string;
  roomType: string;
  hotelId: number;
  hotelName: string;
  checkInDate: Date;
  checkOutDate: Date;
  numberOfGuests: number;
  totalPrice: number;
  status: string;
  specialRequests?: string;
  createdAt: Date;
}

export interface CreateBooking {
  roomId: number;
  checkInDate: Date;
  checkOutDate: Date;
  numberOfGuests: number;
  specialRequests?: string;
}

export interface UpdateBooking {
  checkInDate: Date;
  checkOutDate: Date;
  numberOfGuests: number;
  specialRequests?: string;
}
