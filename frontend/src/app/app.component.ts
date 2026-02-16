import { Component } from '@angular/core';

/**
 * Root application component
 */
@Component({
  selector: 'app-root',
  template: `
    <app-navbar></app-navbar>
    <router-outlet></router-outlet>
  `,
  styles: []
})
export class AppComponent {
  title = 'Hotel Booking';
}
