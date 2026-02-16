import { Routes } from '@angular/router';
import { Home } from './components/home/home';
import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { HotelList } from './components/hotel-list/hotel-list';
import { HotelDetails } from './components/hotel-details/hotel-details';
import { BookingForm } from './components/booking-form/booking-form';
import { UserProfile } from './components/user-profile/user-profile';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  { path: 'hotels', component: HotelList },
  { path: 'hotels/:id', component: HotelDetails },
  { path: 'booking/:roomId', component: BookingForm },
  { path: 'profile', component: UserProfile },
  { path: '**', redirectTo: '' }
];
