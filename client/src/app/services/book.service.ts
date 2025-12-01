import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Book {
  id: number;
  title: string;
  author: string;
  publishedDate: string; // ISO date string
}

@Injectable({
  providedIn: 'root'
})
export class BookService {

  // Base URL to the API
  private apiUrl = 'http://localhost:5062/api/books';

  constructor(private http: HttpClient) { }

  // Reads token from local storage and returns headers.
  // TEMPORARY — will be removed when interceptor is added.
  private getAuthHeaders() {
    const token = localStorage.getItem('auth_token');
    return {
      Authorization: `Bearer ${token}`
    };
  }

  // Gets all books from API.
  getAllBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.apiUrl, {
      headers: this.getAuthHeaders()
    });
  }

  // Gets a single book by id.
  getBook(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  // Creates a new book record.
  createBook(book: Partial<Book>): Observable<Book> {
    return this.http.post<Book>(this.apiUrl, book, {
      headers: this.getAuthHeaders()
    });
  }

  // Updates a book by id.
  updateBook(id: number, book: Partial<Book>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, book, {
      headers: this.getAuthHeaders()
    });
  }

  // Deletes a book by id.
  deleteBook(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }
}


