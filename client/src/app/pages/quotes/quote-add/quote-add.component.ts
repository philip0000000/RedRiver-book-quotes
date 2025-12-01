import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { QuotesService } from '../../../services/quotes.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faSave } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-quote-add',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink, FontAwesomeModule],
  templateUrl: './quote-add.component.html'
})
export class QuoteAddComponent {

  faSave = faSave;

  form: any; // declare but do NOT initialize

  constructor(
    private fb: FormBuilder,
    private quotesService: QuotesService,
    private router: Router
  ) {
    // Initialize form AFTER FormBuilder is injected
    this.form = this.fb.group({
      text: ['', Validators.required]
    });
  }

  // Sends a POST request to create a new quote.
  // If successful, the user is redirected back to the quotes list.
  onSubmit(): void {
    if (this.form.invalid)
      return; // Stop if the form has validation errors

    const text = this.form.value.text ?? '';

    this.quotesService.addQuote(text).subscribe({
      next: () => {
        alert('Citat tillagt!');            // Notify user
        this.router.navigate(['/quotes']);  // Go back to the list
      },
      error: () => alert('Kunde inte lägga till citatet.') // API request failed
    });
  }
}


