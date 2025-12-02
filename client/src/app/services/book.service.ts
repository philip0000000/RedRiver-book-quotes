import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
//import { environment } from '../../environments/environment';

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
  private apiUrl = `${(window as any).environment.apiUrl}/books`;

  constructor(private http: HttpClient) {}

  // Gets all books from API.
  getAllBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.apiUrl);
  }

  // Gets a single book by id.
  getBook(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/${id}`);
  }

  // Creates a new book record.
  createBook(book: Partial<Book>): Observable<Book> {
    return this.http.post<Book>(this.apiUrl, book);
  }

  // Updates a book by id.
  updateBook(id: number, book: Partial<Book>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, book);
  }

  // Deletes a book by id.
  deleteBook(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}


