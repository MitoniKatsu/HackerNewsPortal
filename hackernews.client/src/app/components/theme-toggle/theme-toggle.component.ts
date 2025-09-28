import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { Store } from '@ngxs/store';
import { NewsActions } from '../../store/news/news.actions';

@Component({
  selector: 'app-theme-toggle',
  imports: [MatIconModule, MatSlideToggleModule],
  templateUrl: './theme-toggle.component.html',
  styleUrl: './theme-toggle.component.scss',
})
export class ThemeToggleComponent {
  constructor(private store: Store) {}
  handleThemeChange() {
    this.store.dispatch(new NewsActions.ToggleTheme()).subscribe();
  }
}
