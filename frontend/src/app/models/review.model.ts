export interface Review {
  id: number;
  userId: number;
  userName: string;
  hotelId: number;
  hotelName: string;
  rating: number;
  title: string;
  comment: string;
  createdAt: Date;
  isApproved: boolean;
}

export interface CreateReview {
  hotelId: number;
  rating: number;
  title: string;
  comment: string;
}

export interface UpdateReview {
  rating: number;
  title: string;
  comment: string;
}
