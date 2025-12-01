import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html'
})
export class RegisterComponent {

  form: any; // Will hold the register form

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    // Build the form with fields and validation rules
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  // Sends register request and redirects user to login page.
  onSubmit(): void {
    if (this.form.invalid) // Form not valid
      return;

    // Build a safe object for API request
    const data = {
      username: this.form.value.username ?? '',
      password: this.form.value.password ?? ''
    };

    // Try to register the new user
    this.auth.register(data).subscribe({
      next: () => {
        alert('Konto skapat! Logga in nu.'); // Tell user account is ready
        this.router.navigate(['/login']);    // Go to login page
      },
      error: () => {
        // Show message when register fails
        alert('Något fel har uppstått! Kunde inte skapa konto.');
      }
    });
  }
}


