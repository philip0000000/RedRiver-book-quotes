import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-book-edit',
  standalone: true,
  template: `
    <h2>Edit Book</h2>
    <p>Book ID: {{ id }}</p>
  `
})
export class BookEditComponent implements OnInit {
  id: string | null = null;   // Store the id later

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    // Read id after component is created
    this.id = this.route.snapshot.paramMap.get('id');
  }
}
