import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
//import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'   // Service is available everywhere
})

export class AuthService {

  private apiUrl = `${(window as any).environment.apiUrl}/auth`;

  constructor(private http: HttpClient) {}

  // Sends login request to the API and returns a token. // Call backend login endpoint
  login(data: { username: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, data);
  }

  // Sends register request to the API. // Call backend register endpoint
  register(data: { username: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  // Saves the JWT token in local storage. // Save token locally so user stays logged in
  saveToken(token: string): void {
    localStorage.setItem('auth_token', token);
  }

  // Reads the JWT token from local storage. // Get token if user is logged in
  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  // Check if user is logged in // Token exists = logged in
  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  // Remove token to log out
  logout(): void {
    localStorage.removeItem('auth_token');
  }
}


