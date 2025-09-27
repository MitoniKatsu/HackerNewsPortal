import { Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-news-story',
  imports: [MatCardModule, MatDividerModule, MatListModule],
  templateUrl: './news-story.component.html',
  styleUrl: './news-story.component.scss',
})
export class NewsStoryComponent {
  readonly title = input<string>();
  readonly url = input<string>();
}
