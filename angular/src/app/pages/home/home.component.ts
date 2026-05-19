import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Coffee } from '../../models/coffee';
import { CoffeeService } from '../../services/coffee.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  private svc = inject(CoffeeService);
  coffees = signal<Coffee[] | null>(null);
  error = signal<string | null>(null);

  constructor() {
    this.svc.list().subscribe({
      next: (data) => this.coffees.set(data),
      error: () => this.error.set('Failed to load coffees')
    });
  }
}
