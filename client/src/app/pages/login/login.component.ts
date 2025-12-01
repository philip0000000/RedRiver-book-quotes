import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html'
})
export class LoginComponent {

  form: any; // Will hold the login form

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    // Create the form after fb is ready
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  // Sends login request and redirects user on success.
  onSubmit(): void {
    if (this.form.invalid) // Stop when form is not valid
      return;

    // Build a safe object for API request
    const data = {
      username: this.form.value.username ?? '',
      password: this.form.value.password ?? ''
    };

    // Try to log in the user
    this.auth.login(data).subscribe({
      next: (res) => {
        this.auth.saveToken(res.token);   // Save JWT token
        this.router.navigate(['/books']); // Go to book list
      },
      error: () => {
        // Show message when login fails
        alert('Inloggningen misslyckades. Kontrollera användarnamn eller lösenord.');
      }
    });
  }
}


