import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BookService } from '../../../services/book.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-book-add',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink, FontAwesomeModule],
  templateUrl: './book-add.component.html'
})
export class BookAddComponent {

  // Icon for "Add"
  faPlus = faPlus;

  // Create form with validation rules
  form: any;

  constructor(
    private fb: FormBuilder,
    private bookService: BookService,
    private router: Router
  ) {
    // Build the form fields
    this.form = this.fb.group({
      title: ['', Validators.required],
      author: ['', Validators.required],
      publishedDate: ['', Validators.required]
    });
  }

  // Sends POST to create a new book.
  onSubmit(): void {
    if (this.form.invalid) return; // Stop when form not valid

    // Build request object
    const data = {
      title: this.form.value.title ?? '',
      author: this.form.value.author ?? '',
      publishedDate: this.form.value.publishedDate ?? ''
    };

    // Send POST request to API
    this.bookService.createBook(data).subscribe({
      next: () => {
        alert('Bok skapad!');  // Tell the user that the book is created
        this.router.navigate(['/books']); // Go back to list
      },
      error: () => {
        alert('Kunde inte skapa boken.');
      }
    });
  }
}


