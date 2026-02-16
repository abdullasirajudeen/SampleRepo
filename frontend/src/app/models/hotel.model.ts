export interface Hotel {
  id: number;
  name: string;
  description: string;
  address: string;
  city: string;
  country: string;
  zipCode?: string;
  phoneNumber?: string;
  email?: string;
  starRating: number;
  imageUrl?: string;
  latitude: number;
  longitude: number;
  isActive: boolean;
  averageRating?: number;
  reviewCount?: number;
}

export interface CreateHotel {
  name: string;
  description: string;
  address: string;
  city: string;
  country: string;
  zipCode?: string;
  phoneNumber?: string;
  email?: string;
  starRating: number;
  imageUrl?: string;
  latitude: number;
  longitude: number;
}

export interface UpdateHotel {
  name: string;
  description: string;
  address: string;
  city: string;
  country: string;
  zipCode?: string;
  phoneNumber?: string;
  email?: string;
  starRating: number;
  imageUrl?: string;
  latitude: number;
  longitude: number;
  isActive: boolean;
}
