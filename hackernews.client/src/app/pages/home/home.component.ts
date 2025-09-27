import { CommonModule } from '@angular/common';
import {
  Component,
  computed,
  OnInit,
  Signal,
  signal,
  WritableSignal,
} from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { Store } from '@ngxs/store';
import { NewsActions } from '../../store/news/news.actions';
import { NewsState } from '../../store/news/news.state';
import { NewsStoryComponent } from '../../components/news-story/news-story.component';
import { PagedResponse } from '../../models/paged-response.model';
import { NewsStory } from '../../models/news-story.model';
import { LoadingComponent } from '../../components/loading/loading.component';

@Component({
  selector: 'app-home',
  imports: [
    CommonModule,
    MatCardModule,
    MatPaginatorModule,
    NewsStoryComponent,
    LoadingComponent,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  constructor(private store: Store) {}
  latestNews: Signal<PagedResponse<NewsStory> | null> = signal(null);

  loading: Signal<boolean> = signal(true);
  pageSize: WritableSignal<number> = signal(10);
  initialLoadComplete: WritableSignal<boolean> = signal(false);

  // computed
  stories: Signal<NewsStory[]> = computed(() => this.latestNews()?.page ?? []);
  length: Signal<number | null> = computed(() => this.latestNews()?.total ?? 0);
  pagination: Signal<string> = computed(() =>
    this.stories().length > 0 ? 'with-paginator' : 'without-paginator'
  );

  ngOnInit(): void {
    this.loading = this.store.selectSignal(NewsState.getLoadingHome);
    this.latestNews = this.store.selectSignal(NewsState.getLatestNewsPage);
    this.store
      .dispatch(new NewsActions.GetLatestNews({ pageNumber: 1, pageSize: 10 }))
      .subscribe(() => this.initialLoadComplete.set(true));
  }

  handlePageEvent(e: PageEvent) {
    this.pageSize.set(e.pageSize);
    this.store
      .dispatch(
        new NewsActions.GetLatestNews({
          pageNumber: e.pageIndex + 1,
          pageSize: e.pageSize,
        })
      )
      .subscribe();
  }
}
