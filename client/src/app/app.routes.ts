import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { BookListComponent } from './pages/books/book-list/book-list.component';
import { BookAddComponent } from './pages/books/book-add/book-add.component';
import { BookEditComponent } from './pages/books/book-edit/book-edit.component';
import { QuotesListComponent } from './pages/quotes/quotes-list/quotes-list.component';
import { QuoteAddComponent } from './pages/quotes/quote-add/quote-add.component';
import { QuoteEditComponent } from './pages/quotes/quote-edit/quote-edit.component';
import { AuthGuard } from './guards/auth.guard';
import { inject } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';

export const routes: Routes = [
  // Redirect root to login page
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  // Public routes
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [(route, state) => {   // Stops logged-in users from opening this page
      const auth = inject(AuthService);
      const router = inject(Router);

      if (auth.isLoggedIn()) {
        router.navigate(['/books']);
        return false;
      }

      return true;
    }]
  },
  {
    path: 'register',
    component: RegisterComponent,       // Stops logged-in users from opening this page
    canActivate: [(route, state) => {
      const auth = inject(AuthService);
      const router = inject(Router);

      if (auth.isLoggedIn()) {
        router.navigate(['/books']);
        return false;
      }

      return true;
    }]
  },

  // ===== Protected routes =====
  // Books
  { path: 'books', component: BookListComponent, canActivate: [AuthGuard] },
  { path: 'books/add', component: BookAddComponent, canActivate: [AuthGuard] },
  { path: 'books/edit/:id', component: BookEditComponent, canActivate: [AuthGuard] },

  // Quotes
  { path: 'quotes', component: QuotesListComponent, canActivate: [AuthGuard] },
  { path: 'quotes/add', component: QuoteAddComponent, canActivate: [AuthGuard] },
  { path: 'quotes/edit/:id', component: QuoteEditComponent, canActivate: [AuthGuard] },

  // Fallback route
  { path: '**', redirectTo: 'login' }
];


