import { Component, computed, OnInit, signal, Signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RouterOutlet } from '@angular/router';
import { Store } from '@ngxs/store';
import { NewsState } from './store/news/news.state';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [CommonModule, NavbarComponent, RouterOutlet],
})
export class AppComponent implements OnInit {
  constructor(private store: Store) {}

  storedTheme: Signal<string> = signal('dark');
  theme = computed(() => {
    if (this.storedTheme() === 'dark') {
      return 'dark-mode';
    } else {
      return 'light-mode';
    }
  });

  ngOnInit() {
    this.storedTheme = this.store.selectSignal(NewsState.getTheme);
  }
}
