import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { BookListComponent } from './pages/books/book-list/book-list.component';
import { BookAddComponent } from './pages/books/book-add/book-add.component';
import { BookEditComponent } from './pages/books/book-edit/book-edit.component';
import { QuotesComponent } from './pages/quotes/quotes.component';

export const routes: Routes = [
  // Login
  { path: 'login', component: LoginComponent },

  // Register
  { path: 'register', component: RegisterComponent },

  // Books
  { path: 'books', component: BookListComponent },
  { path: 'books/add', component: BookAddComponent },
  { path: 'books/edit/:id', component: BookEditComponent },

  // Quotes
  { path: 'quotes', component: QuotesComponent },

  // Default route
  { path: '', redirectTo: 'books', pathMatch: 'full' },

  // Wildcard
  { path: '**', redirectTo: 'books' }
];
