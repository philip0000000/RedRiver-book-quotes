import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { QuotesService } from '../../../services/quotes.service';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faSave } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-quote-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, FontAwesomeModule],
  templateUrl: './quote-edit.component.html'
})
export class QuoteEditComponent {

  faSave = faSave; // Icon used on the save button

  form: any; // declare only

  quoteId: number = 0; // Id of the quote being edited

  constructor(
    private route: ActivatedRoute,
    private quotesService: QuotesService,
    private fb: FormBuilder,
    private router: Router
  ) {
    // Build the form after FormBuilder is injected
    this.form = this.fb.group({
      text: ['', Validators.required]
    });
  }

  // Load the quote when the component initializes
  ngOnInit(): void {
    this.quoteId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadQuote();
  }

  // Fetch quote data from the API and populate the form
  loadQuote(): void {
    this.quotesService.getQuote(this.quoteId).subscribe({
      next: (quote) => {
        this.form.patchValue({
          text: quote.text
        });
      },
      error: () => {
        // If quote does not exist or the user does not own it
        alert('Kunde inte ladda citatet.');
        this.router.navigate(['/quotes']);
      }
    });
  }

  // Submit updated quote text to the API
  onSubmit(): void {
    if (this.form.invalid)
      return; // Prevent sending invalid/empty data

    const newText = this.form.value.text ?? '';

    this.quotesService.updateQuote(this.quoteId, newText).subscribe({
      next: () => {
        alert('Citat uppdaterat!');
        this.router.navigate(['/quotes']); // Return to quotes list
      },
      error: () => alert('Kunde inte uppdatera citatet.') // API failure
    });
  }
}


