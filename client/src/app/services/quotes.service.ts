import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface Quote {
  id: number;
  text: string;
  userId: number;
}

@Injectable({
  providedIn: 'root'
})

export class QuotesService {

  private apiUrl = `${environment.apiUrl}/quotes`;

  constructor(private http: HttpClient) { }

  // Get all quotes for current user
  getMyQuotes(): Observable<Quote[]> {
    return this.http.get<Quote[]>(this.apiUrl);
  }

  // Get single quote by id
  getQuote(id: number): Observable<Quote> {
    return this.http.get<Quote>(`${this.apiUrl}/${id}`);
  }

  // Create new quote
  addQuote(text: string): Observable<Quote> {
    return this.http.post<Quote>(this.apiUrl, { text });
  }

  // Update quote
  updateQuote(id: number, text: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, { text });
  }

  // Delete quote
  deleteQuote(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}


