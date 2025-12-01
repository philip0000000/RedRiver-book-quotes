import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from './services/auth.service';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ThemeService } from './services/theme.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CommonModule,
    RouterLink
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})

export class App {

  constructor(
    public auth: AuthService,   // Used in the navbar template
    private router: Router,
    private theme: ThemeService
  ) {
    // Load saved dark/light mode theme or default to light
    const saved = localStorage.getItem('app_theme');
    if (!saved) {
      document.body.classList.add('light');
    } else {
      document.body.classList.add(saved);
    }
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  // Change between dark and light mode
  toggleTheme() {
    this.theme.toggleTheme();
  }
}


