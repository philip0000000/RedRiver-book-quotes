import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { QuotesService, Quote } from '../../../services/quotes.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faPlus, faPen, faTrash } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-quotes-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FontAwesomeModule],
  templateUrl: './quotes-list.component.html'
})
export class QuotesListComponent {

  quotes: Quote[] = []; // Holds the quotes returned from the API

  // Icons used in the UI
  faPlus = faPlus;
  faEdit = faPen;
  faDelete = faTrash;

  constructor(
    private quotesService: QuotesService,
    private router: Router
  ) { }

  // Load all quotes when component is initialized
  ngOnInit(): void {
    this.loadQuotes();
  }

  // Fetch all quotes belonging to the logged-in user
  loadQuotes(): void {
    this.quotesService.getMyQuotes().subscribe({
      next: (res) => this.quotes = res,
      error: () => alert('Kunde inte ladda citat.')
    });
  }

  // Delete a quote and refresh the list
  deleteQuote(id: number): void {
    // Confirm user action before deleting
    if (!confirm('Är du säker på att du vill radera detta citat?'))
      return;

    this.quotesService.deleteQuote(id).subscribe({
      next: () => {
        alert('Citat raderades.');
        this.loadQuotes();          // Refresh list after deletion
      },
      error: () => alert('Kunde inte radera citatet.') // Handle delete error
    });
  }
}


