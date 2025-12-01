import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {

  private themeKey = 'app_theme'; // storage key

  constructor() {
    // Load theme on startup
    const saved = localStorage.getItem(this.themeKey);
    if (saved) {
      this.applyTheme(saved);
    }
  }

  // Switch between 'light' and 'dark'
  toggleTheme() {
    const current = this.getTheme();
    const newTheme = current === 'dark' ? 'light' : 'dark';

    this.applyTheme(newTheme);
    localStorage.setItem(this.themeKey, newTheme);
  }

  // Read current theme
  getTheme(): string {
    return localStorage.getItem(this.themeKey) || 'light';
  }

  // Add theme class to <body>
  private applyTheme(theme: string) {
    document.body.classList.remove('light', 'dark');
    document.body.classList.add(theme);
  }
}


