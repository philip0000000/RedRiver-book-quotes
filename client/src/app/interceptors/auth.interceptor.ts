import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';

// Global HTTP interceptor that automatically attaches the JWT token
// to all outgoing API requests that require authentication.

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  // Read token from AuthService (localStorage under the hood)
  const auth = inject(AuthService);
  const token = auth.getToken();

  // If user is not logged in, forward the original request
  if (!token) {
    return next(req);
  }

  // Clone request and add Authorization header
  const cloned = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  // Continue pipeline with modified request
  return next(cloned);
};


