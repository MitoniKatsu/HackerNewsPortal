import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';
import { SearchComponent } from './search.component';
import { provideStore, Store, TypedSelector } from '@ngxs/store';
import {
  computed,
  CUSTOM_ELEMENTS_SCHEMA,
  Signal,
  signal,
  WritableSignal,
} from '@angular/core';
import { mockNewsState } from '../../store/news/news.mock';
import { NewsState, NewsStateModel } from '../../store/news/news.state';
import { PagedResponse } from '../../models/paged-response.model';
import { RankedNewsStory } from '../../models/ranked-news-story.model';
import { of } from 'rxjs';
import { NewsActions } from '../../store/news/news.actions';
import { PageEvent } from '@angular/material/paginator';
import { By } from '@angular/platform-browser';

describe('SearchComponent', () => {
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;
  let store: Store;
  let fakeLoading = signal(false);
  let fakeRankedNews: WritableSignal<PagedResponse<RankedNewsStory> | null> =
    signal(null);
  let init: NewsStateModel = {
    theme: '',
    loadingHome: false,
    loadingSearch: false,
    latestNews: null,
    rankedNews: null,
  };

  beforeEach(
    async () =>
      await TestBed.configureTestingModule({
        imports: [SearchComponent],
        providers: [provideStore([])],
        schemas: [CUSTOM_ELEMENTS_SCHEMA],
      }).compileComponents()
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchComponent);
    component = fixture.componentInstance;
    store = TestBed.inject(Store);
    spyOn(store, 'dispatch').and.returnValue(of());
    spyOn(store, 'selectSignal')
      .withArgs(NewsState.getLoadingSearch)
      .and.returnValue(computed(fakeLoading))
      .withArgs(NewsState.getRankedNewsPage)
      .and.returnValue(computed(fakeRankedNews));
    fixture.detectChanges(); // for onInit
  });

  afterEach(() => {
    component.searchString.set('');
    fakeLoading.set(false);
    fakeRankedNews.set(null);
    store.reset({ news: init });
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display loading spinner when loading is true', () => {
    fakeLoading.set(true);
    fixture.detectChanges();
    let spinner = fixture.nativeElement.querySelector('app-loading');
    expect(spinner).not.toBeNull();
  });

  it('should not display loading spinner when loading is false', () => {
    let spinner = fixture.nativeElement.querySelector('app-loading');
    expect(spinner).toBeNull();
  });
  it('should hide paginator if no search is submitted', () => {
    let paginator = fixture.nativeElement.querySelector(
      'mat-card-actions.paginator'
    );
    let content = fixture.nativeElement.querySelector(
      'mat-card-content.without-paginator'
    );
    expect(paginator).toBeNull();
    expect(content).not.toBeNull();
  });
  it('should not hide paginator if a search is submitted', () => {
    fakeRankedNews.set({
      page: [{ id: 1, title: 'test', url: 'test', searchRanking: 1 }],
      pageNumber: 1,
      pageCount: 1,
      total: 10,
    });
    component.searchSubmitted.set(true);
    fixture.detectChanges();
    let paginator = fixture.nativeElement.querySelector(
      'mat-card-actions.paginator'
    );
    let content = fixture.nativeElement.querySelector(
      'mat-card-content.with-paginator'
    );
    expect(paginator).not.toBeNull();
    expect(content).not.toBeNull();
  });

  it('should dispatch search request based on searchString when handleSearchSubmit is called', fakeAsync(() => {
    let input = fixture.debugElement.query(By.css('input'));
    input.nativeElement.value = 'testing';
    input.nativeElement.dispatchEvent(new Event('input')); //update the ngmodel
    input.triggerEventHandler('change', null);
    tick();
    expect(component.searchString()).toBe('testing');
    expect(store.dispatch).toHaveBeenCalledWith(
      new NewsActions.GetRankedNews({
        pageNumber: 1,
        pageSize: 10,
        searchString: component.searchString(),
      })
    );
  }));

  it('should dispatch search request based on PageEvent when handlePageEvent is called', () => {
    component.searchString.set('test');
    fixture.detectChanges();
    let event: PageEvent = {
      pageIndex: 4,
      pageSize: 20,
      length: 100,
    };
    component.handlePageEvent(event);
    fixture.detectChanges();
    expect(component.pageSize()).toBe(event.pageSize);
    expect(store.dispatch).toHaveBeenCalledWith(
      new NewsActions.GetRankedNews({
        pageNumber: event.pageIndex + 1,
        pageSize: event.pageSize,
        searchString: 'test',
      })
    );
  });

  it('should reset search on destroy', () => {
    component.ngOnDestroy();
    expect(store.dispatch).toHaveBeenCalledWith(new NewsActions.ResetSearch());
  });
});
