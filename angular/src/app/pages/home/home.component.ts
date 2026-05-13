import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Coffee, Roast } from '../../models/coffee';
import { CoffeeService } from '../../services/coffee.service';

type SortableKey = 'name' | 'origin' | 'body' | 'bitterness' | 'price';

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
  sortKey = signal<SortableKey | null>(null);
  roastFilter = signal<Roast | null>(null);

  constructor() {
    this.svc.list().subscribe({
      next: (data) => this.coffees.set(data),
      error: () => this.error.set('Failed to load coffees')
    });
  }

  sortedCoffees = computed(() => {
    const key = this.sortKey();
    const roast = this.roastFilter();
    const coffees = this.coffees();
    if (!coffees) return coffees;
    const filtered = roast ? coffees.filter(c => c.roast === roast) : coffees;
    if (!key) return filtered;
    return [...filtered].sort((a, b) => {
      const av = a[key], bv = b[key];
      if (typeof av === 'number' && typeof bv === 'number') return av - bv;
      return String(av).localeCompare(String(bv));
    });
  });
}
