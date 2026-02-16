/**
 * User model representing a user entity
 */
export interface User {
  id: number;
  email: string;
  fullName: string;
  phoneNumber?: string;
  role: string;
  createdAt: Date;
}

/**
 * RegisterUserDto for user registration
 */
export interface RegisterUserDto {
  email: string;
  fullName: string;
  password: string;
  phoneNumber?: string;
}

/**
 * LoginDto for user authentication
 */
export interface LoginDto {
  email: string;
  password: string;
}

/**
 * UserDto for user data transfer
 */
export interface UserDto {
  id: number;
  email: string;
  fullName: string;
  phoneNumber?: string;
  role: string;
  createdAt: Date;
}
