import { CommonModule } from '@angular/common';
import {
  Component,
  computed,
  OnDestroy,
  OnInit,
  Signal,
  signal,
  WritableSignal,
} from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Store } from '@ngxs/store';
import { NewsActions } from '../../store/news/news.actions';
import { NewsStoryComponent } from '../../components/news-story/news-story.component';
import { PagedResponse } from '../../models/paged-response.model';
import { RankedNewsStory } from '../../models/ranked-news-story.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { NewsState } from '../../store/news/news.state';
import { LoadingComponent } from '../../components/loading/loading.component';

@Component({
  selector: 'app-search',
  imports: [
    CommonModule,
    MatCardModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    NewsStoryComponent,
    FormsModule,
    LoadingComponent,
  ],
  templateUrl: './search.component.html',
  styleUrl: './search.component.scss',
})
export class SearchComponent implements OnInit, OnDestroy {
  constructor(private store: Store) {}

  rankedNews: Signal<PagedResponse<RankedNewsStory> | null> = signal(null);

  loading: Signal<boolean> = signal(false);
  pageSize: WritableSignal<number> = signal(10);
  searchString: WritableSignal<string> = signal('');
  searchSubmitted: WritableSignal<boolean> = signal(false);

  // computed
  stories: Signal<RankedNewsStory[]> = computed(
    () => this.rankedNews()?.page ?? []
  );
  length: Signal<number | null> = computed(() => this.rankedNews()?.total ?? 0);
  pagination: Signal<string> = computed(() =>
    this.stories().length > 0 ? 'with-paginator' : 'without-paginator'
  );

  ngOnInit(): void {
    this.loading = this.store.selectSignal(NewsState.getLoadingSearch);
    this.rankedNews = this.store.selectSignal(NewsState.getRankedNewsPage);
  }

  ngOnDestroy(): void {
    this.store.dispatch(new NewsActions.ResetSearch()).subscribe();
  }

  handlePageEvent(e: PageEvent) {
    this.pageSize.set(e.pageSize);
    this.store
      .dispatch(
        new NewsActions.GetRankedNews({
          pageNumber: e.pageIndex + 1,
          pageSize: e.pageSize,
          searchString: this.searchString(),
        })
      )
      .subscribe();
  }

  handleSearchSubmit() {
    this.searchSubmitted.set(true);
    this.store
      .dispatch(
        new NewsActions.GetRankedNews({
          pageNumber: 1,
          pageSize: 10,
          searchString: this.searchString(),
        })
      )
      .subscribe();
  }
}
