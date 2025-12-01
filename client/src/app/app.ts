import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from './services/auth.service';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

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
    private router: Router
  ) {}

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}


