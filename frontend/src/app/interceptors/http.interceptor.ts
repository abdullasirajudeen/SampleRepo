import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

/**
 * HTTP interceptor for handling errors and adding authentication tokens
 */
@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  /**
   * Intercepts HTTP requests and responses
   * @param request HTTP request
   * @param next HTTP handler
   * @returns Observable of HTTP event
   */
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Clone the request and add authentication token if available
    const currentUser = localStorage.getItem('currentUser');
    if (currentUser) {
      // In a real application, you would add the JWT token here
      // request = request.clone({
      //   setHeaders: {
      //     Authorization: `Bearer ${token}`
      //   }
      // });
    }

    // Add common headers only if not already set and request has a JSON body
    const headers: { [key: string]: string } = {
      'Accept': 'application/json'
    };
    
    // Only set Content-Type if the request body is JSON and header is not already set
    if (request.body && !request.headers.has('Content-Type')) {
      headers['Content-Type'] = 'application/json';
    }

    request = request.clone({
      setHeaders: headers
    });

    // Handle the request and catch any errors
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'An unknown error occurred';

        if (error.error instanceof ErrorEvent) {
          // Client-side error
          errorMessage = `Client Error: ${error.error.message}`;
          console.error('Client-side error:', error.error.message);
        } else {
          // Server-side error
          errorMessage = `Server Error: ${error.status} - ${error.message}`;
          console.error('Server-side error:', {
            status: error.status,
            message: error.message,
            body: error.error
          });

          // Handle specific error codes
          switch (error.status) {
            case 400:
              errorMessage = error.error?.message || 'Bad request. Please check your input.';
              break;
            case 401:
              errorMessage = 'Unauthorized. Please log in.';
              // Optionally redirect to login page
              break;
            case 403:
              errorMessage = 'Forbidden. You do not have permission to access this resource.';
              break;
            case 404:
              errorMessage = error.error?.message || 'Resource not found.';
              break;
            case 500:
              errorMessage = 'Internal server error. Please try again later.';
              break;
            case 503:
              errorMessage = 'Service unavailable. Please try again later.';
              break;
          }
        }

        // Log the error for debugging
        console.error('HTTP Error:', errorMessage);

        // Return an observable with a user-facing error message
        return throwError(() => ({
          error: errorMessage,
          status: error.status,
          originalError: error
        }));
      })
    );
  }
}
