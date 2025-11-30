import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'   // Service is available everywhere
})
export class AuthService {

  private apiUrl = 'http://localhost:5062/api/auth';
  // Change later when deploying

  constructor(private http: HttpClient) { }

  /// <summary>
  /// Sends login request to the API and returns a token.
  /// </summary>
  login(data: { username: string; password: string }): Observable<any> {
    // Call backend login endpoint
    return this.http.post(`${this.apiUrl}/login`, data);
  }

  /// <summary>
  /// Sends register request to the API.
  /// </summary>
  register(data: { username: string; password: string }): Observable<any> {
    // Call backend register endpoint
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  /// <summary>
  /// Saves the JWT token in local storage.
  /// </summary>
  saveToken(token: string): void {
    // Save token locally so user stays logged in
    localStorage.setItem('auth_token', token);
  }

  /// <summary>
  /// Reads the JWT token from local storage.
  /// </summary>
  getToken(): string | null {
    // Get token if user is logged in
    return localStorage.getItem('auth_token');
  }

  /// <summary>
  /// Removes token from local storage.
  /// </summary>
  logout(): void {
    // Clear the user token to log out
    localStorage.removeItem('auth_token');
  }
}
