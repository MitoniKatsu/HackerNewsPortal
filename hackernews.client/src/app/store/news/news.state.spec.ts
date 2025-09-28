import { TestBed } from '@angular/core/testing';
import { provideStore, Store } from '@ngxs/store';
import { NewsState, NewsStateModel } from './news.state';
import { NewsService } from '../../services/news.service';
import {
  HttpErrorResponse,
  HttpResponse,
  provideHttpClient,
} from '@angular/common/http';
import { NewsActions } from './news.actions';
import { catchError, of, throwError } from 'rxjs';
import { PagedResponse } from '../../models/paged-response.model';
import { NewsStory } from '../../models/news-story.model';
import { Signal } from '@angular/core';
import { RankedNewsStory } from '../../models/ranked-news-story.model';
import { provideToastr, ToastrService } from 'ngx-toastr';

describe('News store', () => {
  let newsService: NewsService;
  let store: Store;
  let toast: ToastrService;
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideStore([NewsState]),
        provideHttpClient(),
        NewsService,
        provideToastr(),
      ],
    });

    store = TestBed.inject(Store);
    newsService = TestBed.inject(NewsService);
    toast = TestBed.inject(ToastrService);
  });

  describe('NewsActions.GetLatestNews', () => {
    it('should patch state with response when response is ok', (done) => {
      const fakeResponse = new HttpResponse<PagedResponse<NewsStory>>();
      spyOn(newsService, 'getLatestNews').and.returnValue(of(fakeResponse));
      store
        .dispatch(
          new NewsActions.GetLatestNews({ pageNumber: 1, pageSize: 10 })
        )
        .subscribe(() => {
          expect(newsService.getLatestNews).toHaveBeenCalledOnceWith(1, 10);
          const latestNewsState: Signal<PagedResponse<NewsStory> | null> =
            store.selectSignal(NewsState.getLatestNewsPage);
          const loadingState: Signal<boolean> = store.selectSignal(
            NewsState.getLoadingHome
          );
          expect(latestNewsState()).toEqual(fakeResponse.body);
          expect(loadingState()).toBe(false);
          done();
        });
    });

    it('should not patch state with response when response is not ok', (done) => {
      const error = 'It Broke';
      spyOn(newsService, 'getLatestNews').and.returnValue(
        throwError(() => new Error(error))
      );
      spyOn(toast, 'error');
      store
        .dispatch(
          new NewsActions.GetLatestNews({ pageNumber: 1, pageSize: 10 })
        )
        .subscribe({
          error: () => {
            expect(newsService.getLatestNews).toHaveBeenCalledOnceWith(1, 10);
            const latestNewsState: Signal<PagedResponse<NewsStory> | null> =
              store.selectSignal(NewsState.getLatestNewsPage);
            const loadingState: Signal<boolean> = store.selectSignal(
              NewsState.getLoadingHome
            );
            expect(latestNewsState()).toBe(null);
            expect(loadingState()).toBe(false);
            expect(toast.error).toHaveBeenCalledWith(
              'Unable to retrieve Latest News',
              'An Error Has Occurred'
            );
            done();
          },
        });
    });
  });

  describe('NewsActions.GetRankedNews', () => {
    it('should patch state with response when response is ok', (done) => {
      const fakeResponse = new HttpResponse<PagedResponse<RankedNewsStory>>();
      spyOn(newsService, 'getRankedNews').and.returnValue(of(fakeResponse));
      store
        .dispatch(
          new NewsActions.GetRankedNews({
            pageNumber: 1,
            pageSize: 10,
            searchString: 'test',
          })
        )
        .subscribe(() => {
          expect(newsService.getRankedNews).toHaveBeenCalledOnceWith(
            1,
            10,
            'test'
          );
          const rankedNewsState: Signal<PagedResponse<RankedNewsStory> | null> =
            store.selectSignal(NewsState.getRankedNewsPage);
          const loadingState: Signal<boolean> = store.selectSignal(
            NewsState.getLoadingSearch
          );
          expect(rankedNewsState()).toEqual(fakeResponse.body);
          expect(loadingState()).toBe(false);
          done();
        });
    });

    it('should not patch state with response when response is not ok', (done) => {
      const error = 'It Broke';
      spyOn(newsService, 'getRankedNews').and.returnValue(
        throwError(() => new Error(error))
      );
      spyOn(toast, 'error');
      store
        .dispatch(
          new NewsActions.GetRankedNews({
            pageNumber: 1,
            pageSize: 10,
            searchString: 'test',
          })
        )
        .subscribe({
          error: () => {
            expect(newsService.getRankedNews).toHaveBeenCalledOnceWith(
              1,
              10,
              'test'
            );
            const rankedNewsState: Signal<PagedResponse<RankedNewsStory> | null> =
              store.selectSignal(NewsState.getRankedNewsPage);
            const loadingState: Signal<boolean> = store.selectSignal(
              NewsState.getLoadingSearch
            );
            expect(rankedNewsState()).toBe(null);
            expect(loadingState()).toBe(false);
            expect(toast.error).toHaveBeenCalledWith(
              'Unable to retrieve results',
              'An Error Has Occurred'
            );
            done();
          },
        });
    });
  });

  describe('NewsActions.ResetSearch', () => {
    beforeEach(() => {
      store.reset({
        ...store.snapshot(),
        loadingSearch: true,
        rankedNews: new PagedResponse<RankedNewsStory>(),
      });
    });

    it('should reset the search state', (done) => {
      store.dispatch(new NewsActions.ResetSearch()).subscribe(() => {
        let rankedNews = store.selectSignal(NewsState.getRankedNewsPage);
        let loading = store.selectSignal(NewsState.getLoadingSearch);
        expect(rankedNews()).toBe(null);
        expect(loading()).toBe(false);
        done();
      });
    });
  });
  describe('NewsActions.ToggleTheme', () => {
    const init: NewsStateModel = {
      theme: '',
      loadingHome: false,
      loadingSearch: false,
      latestNews: null,
      rankedNews: null,
    };
    beforeEach(() => {
      store.reset({ ...store.snapshot() });
    });
    it('should switch dark to light', (done) => {
      init.theme = 'dark';
      store.reset({ news: init });

      store.dispatch(new NewsActions.ToggleTheme()).subscribe(() => {
        let theme = store.selectSignal(NewsState.getTheme);
        expect(theme()).toBe('light');
        done();
      });
    });
    it('should switch light to dark', (done) => {
      init.theme = 'light';
      store.reset({ news: init });

      store.dispatch(new NewsActions.ToggleTheme()).subscribe(() => {
        let theme = store.selectSignal(NewsState.getTheme);
        expect(theme()).toBe('dark');
        done();
      });
    });
  });

  describe('Selectors', () => {
    const init: NewsStateModel = {
      theme: '',
      loadingHome: false,
      loadingSearch: false,
      latestNews: null,
      rankedNews: null,
    };

    beforeEach(() => {
      store.reset({ news: init });
    });

    describe('getLoadingHome', () => {
      it('should return state value', () => {
        let selector = store.selectSignal(NewsState.getLoadingHome);
        expect(selector()).toBe(init.loadingHome);
      });
    });
    describe('getLoadingSearch', () => {
      it('should return state value', () => {
        let selector = store.selectSignal(NewsState.getLoadingSearch);
        expect(selector()).toBe(init.loadingSearch);
      });
    });
    describe('getLatestNewsPage', () => {
      it('should return state value', () => {
        let selector = store.selectSignal(NewsState.getLatestNewsPage);
        expect(selector()).toBe(init.latestNews);
      });
    });
    describe('getRankedNewsPage', () => {
      it('should return state value', () => {
        let selector = store.selectSignal(NewsState.getRankedNewsPage);
        expect(selector()).toBe(init.rankedNews);
      });
    });
    describe('getTheme', () => {
      it('should return state value', () => {
        let selector = store.selectSignal(NewsState.getTheme);
        expect(selector()).toBe(init.theme);
      });
    });
  });
});
