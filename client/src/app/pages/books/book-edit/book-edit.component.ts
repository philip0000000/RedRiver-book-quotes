import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { BookService, Book } from '../../../services/book.service';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faSave, faArrowLeft } from '@fortawesome/free-solid-svg-icons';

// Edit page lets the user update an existing book.
// Loads book data and sends update request.

@Component({
  selector: 'app-book-edit',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink, FontAwesomeModule],
  templateUrl: './book-edit.component.html'
})

export class BookEditComponent implements OnInit {

  // Icons
  faSave = faSave;
  faArrowLeft = faArrowLeft;

  id!: number;
  form: any; // Will hold the form

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private bookService: BookService,
    private router: Router
  ) {
    // Create empty form first
    this.form = this.fb.group({
      title: ['', Validators.required],
      author: ['', Validators.required],
      publishedDate: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // Read book id from URL
    this.id = Number(this.route.snapshot.paramMap.get('id'));

    // Load book data
    this.bookService.getBook(this.id).subscribe({
      next: (book: Book) => {
        // Fill form with book data
        this.form.patchValue({
          title: book.title,
          author: book.author,
          publishedDate: book.publishedDate.split('T')[0]  // Only date part
        });
      },
      error: () => {
        alert('Kunde inte läsa bokdata.');
        this.router.navigate(['/books']);
      }
    });
  }

  // Sends update request to API
  onSubmit(): void {
    if (this.form.invalid) return;

    const data = {
      title: this.form.value.title ?? '',
      author: this.form.value.author ?? '',
      publishedDate: this.form.value.publishedDate ?? ''
    };

    // Send PUT request
    this.bookService.updateBook(this.id, data).subscribe({
      next: () => {
        alert('Boken har uppdaterats!');
        this.router.navigate(['/books']);
      },
      error: () => {
        alert('Kunde inte uppdatera boken.');
      }
    });
  }
}
