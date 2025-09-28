import { Injectable } from '@angular/core';
import { State, Action, Selector, StateContext } from '@ngxs/store';
import { PagedResponse } from '../../models/paged-response.model';
import { NewsStory } from '../../models/news-story.model';
import { RankedNewsStory } from '../../models/ranked-news-story.model';
import { NewsActions } from './news.actions';
import {
  Pagination,
  PaginationWithSearch,
} from '../../models/pagination.model';
import { NewsService } from '../../services/news.service';
import { tap } from 'rxjs';

export interface NewsStateModel {
  theme: string;
  loadingHome: boolean;
  loadingSearch: boolean;
  latestNews: PagedResponse<NewsStory> | null;
  rankedNews: PagedResponse<RankedNewsStory> | null;
}

@State<NewsStateModel>({
  name: 'news',
  defaults: {
    theme: 'dark',
    loadingHome: true,
    loadingSearch: false,
    latestNews: null,
    rankedNews: null,
  },
})
@Injectable()
export class NewsState {
  constructor(private newsService: NewsService) {}
  @Action(NewsActions.GetLatestNews)
  getLatestNews(
    ctx: StateContext<NewsStateModel>,
    action: { payload: Pagination }
  ) {
    ctx.patchState({
      loadingHome: true,
    });
    return this.newsService
      .getLatestNews(action.payload.pageNumber, action.payload.pageSize)
      .pipe(
        tap((res) => {
          if (res.ok) {
            ctx.patchState({
              loadingHome: false,
              latestNews: res.body,
            });
          } else {
            ctx.patchState({
              loadingHome: false,
            });
            console.log('error', res.statusText);
          }
        })
      );
  }

  @Action(NewsActions.GetRankedNews)
  getRankedNews(
    ctx: StateContext<NewsStateModel>,
    action: { payload: PaginationWithSearch }
  ) {
    ctx.patchState({
      loadingSearch: true,
    });
    return this.newsService
      .getRankedNews(
        action.payload.pageNumber,
        action.payload.pageSize,
        action.payload.searchString
      )
      .pipe(
        tap((res) => {
          if (res.ok) {
            ctx.patchState({
              loadingSearch: false,
              rankedNews: res.body,
            });
          } else {
            ctx.patchState({
              loadingSearch: false,
            });
            console.log('error', res.statusText);
          }
        })
      );
  }

  @Action(NewsActions.ResetSearch)
  resetSearch(ctx: StateContext<NewsStateModel>) {
    ctx.patchState({
      loadingSearch: false,
      rankedNews: null,
    });
  }

  @Action(NewsActions.ToggleTheme)
  toggleTheme(ctx: StateContext<NewsStateModel>) {
    const state = ctx.getState();
    if (state.theme === 'dark') {
      ctx.patchState({
        theme: 'light',
      });
    } else {
      ctx.patchState({
        theme: 'dark',
      });
    }
  }

  @Selector()
  static getLoadingHome(state: NewsStateModel) {
    return state.loadingHome;
  }

  @Selector()
  static getLoadingSearch(state: NewsStateModel) {
    return state.loadingSearch;
  }

  @Selector()
  static getLatestNewsPage(state: NewsStateModel) {
    return state.latestNews;
  }

  @Selector()
  static getRankedNewsPage(state: NewsStateModel) {
    return state.rankedNews;
  }

  @Selector()
  static getTheme(state: NewsStateModel) {
    return state.theme;
  }
}
