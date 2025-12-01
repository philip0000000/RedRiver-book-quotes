import { Component, OnInit } from '@angular/core';
import { Book, BookService } from '../../../services/book.service';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faPenToSquare, faTrash, faPlus } from '@fortawesome/free-solid-svg-icons';

// Shows all books and lets the user delete or edit a book.

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FontAwesomeModule],
  templateUrl: './book-list.component.html'
})
export class BookListComponent implements OnInit {

  // Icons
  faPen = faPenToSquare;
  faTrash = faTrash;
  faPlus = faPlus;

  books: Book[] = [];

  constructor(private bookService: BookService) { }

  ngOnInit(): void {
    this.loadBooks();  // Load books when page starts
  }

  // Loads all books from API.
  loadBooks(): void {
    this.bookService.getAllBooks().subscribe({
      next: (data) => {
        this.books = data;
      },
      error: () => {
        alert('Kunde inte läsa böcker.');
      }
    });
  }

  // Deletes a book by id after confirm.
  deleteBook(id: number): void {
    if (!confirm('Är du säker på att du vill radera denna bok?')) {
      return;
    }

    this.bookService.deleteBook(id).subscribe({
      next: () => {
        // Remove deleted book from local list
        this.books = this.books.filter(b => b.id !== id);
        alert('Boken raderades.');
      },
      error: () => {
        alert('Kunde inte radera boken.');
      }
    });
  }
}


