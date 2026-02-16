import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// Components
import { HomeComponent } from './components/home/home.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { HotelListComponent } from './components/hotel-list/hotel-list.component';
import { HotelDetailsComponent } from './components/hotel-details/hotel-details.component';
import { BookingFormComponent } from './components/booking-form/booking-form.component';

// Services
import { ApiService } from './services/api.service';
import { HotelService } from './services/hotel.service';
import { BookingService } from './services/booking.service';
import { AuthService } from './services/auth.service';

// Interceptors
import { HttpErrorInterceptor } from './interceptors/http.interceptor';

/**
 * Main application module
 */
@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavbarComponent,
    HotelListComponent,
    HotelDetailsComponent,
    BookingFormComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    ApiService,
    HotelService,
    BookingService,
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
