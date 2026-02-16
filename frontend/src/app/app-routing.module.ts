import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { HotelListComponent } from './components/hotel-list/hotel-list.component';
import { HotelDetailsComponent } from './components/hotel-details/hotel-details.component';
import { BookingFormComponent } from './components/booking-form/booking-form.component';

/**
 * Application routes configuration
 */
const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'hotels',
    component: HotelListComponent
  },
  {
    path: 'hotels/:id',
    component: HotelDetailsComponent
  },
  {
    path: 'booking',
    component: BookingFormComponent
  },
  {
    path: '**',
    redirectTo: ''
  }
];

/**
 * App routing module
 */
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
