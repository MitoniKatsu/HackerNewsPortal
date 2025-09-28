import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeComponent } from './home.component';
import { provideStore, Store } from '@ngxs/store';
import {
  computed,
  CUSTOM_ELEMENTS_SCHEMA,
  signal,
  WritableSignal,
} from '@angular/core';
import { of } from 'rxjs';
import { NewsState, NewsStateModel } from '../../store/news/news.state';
import { PagedResponse } from '../../models/paged-response.model';
import { NewsStory } from '../../models/news-story.model';
import { NewsActions } from '../../store/news/news.actions';
import { PageEvent } from '@angular/material/paginator';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let store: Store;
  let fakeLoading = signal(true);
  let fakeLatestNews: WritableSignal<PagedResponse<NewsStory> | null> =
    signal(null);
  let init: NewsStateModel = {
    theme: '',
    loadingHome: false,
    loadingSearch: false,
    latestNews: null,
    rankedNews: null,
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HomeComponent],
      providers: [provideStore([])],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    store = TestBed.inject(Store);
    spyOn(store, 'dispatch').and.returnValue(of());
    spyOn(store, 'selectSignal')
      .withArgs(NewsState.getLoadingHome)
      .and.returnValue(computed(fakeLoading))
      .withArgs(NewsState.getLatestNewsPage)
      .and.returnValue(computed(fakeLatestNews));
    fixture.detectChanges(); // for onInit
  });

  afterEach(() => {
    fakeLoading.set(true);
    fakeLatestNews.set(null);
    store.reset({ news: init });
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should dispatch for latest news upon loading', () => {
    expect(store.dispatch).toHaveBeenCalledOnceWith(
      new NewsActions.GetLatestNews({ pageNumber: 1, pageSize: 10 })
    );
  });

  it('should display loading spinner when loading is true', () => {
    let spinner = fixture.nativeElement.querySelector('app-loading');
    expect(spinner).not.toBeNull();
  });

  it('should not display loading spinner when loading is false', () => {
    fakeLoading.set(false);
    fixture.detectChanges();
    let spinner = fixture.nativeElement.querySelector('app-loading');
    expect(spinner).toBeNull();
  });

  it('should hide paginator if initial load is not complete', () => {
    let paginator = fixture.nativeElement.querySelector(
      'mat-card-actions.paginator'
    );
    let content = fixture.nativeElement.querySelector(
      'mat-card-content.without-paginator'
    );
    expect(paginator).toBeNull();
    expect(content).not.toBeNull();
  });
  it('should not hide paginator if initial load is complete', () => {
    fakeLatestNews.set({
      page: [{ id: 1, title: 'test', url: 'test' }],
      pageNumber: 1,
      pageCount: 1,
      total: 10,
    });
    component.initialLoadComplete.set(true);
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

  it('should dispatch latest news request based on PageEvent when handlePageEvent is called', () => {
    let event: PageEvent = {
      pageIndex: 4,
      pageSize: 20,
      length: 100,
    };
    component.handlePageEvent(event);
    fixture.detectChanges();
    expect(component.pageSize()).toBe(event.pageSize);
    expect(store.dispatch).toHaveBeenCalledWith(
      new NewsActions.GetLatestNews({
        pageNumber: event.pageIndex + 1,
        pageSize: event.pageSize,
      })
    );
  });

  it('should display a news story for each story in the currently loaded page', () => {
    fakeLatestNews.set({
      page: [
        { id: 1, title: 'test1', url: 'url1' },
        { id: 2, title: 'test2', url: 'url2' },
        { id: 2, title: 'test3', url: 'url3' },
      ],
      pageNumber: 1,
      pageCount: 10,
      total: 3,
    });
    fakeLoading.set(false);
    component.initialLoadComplete.set(true);
    fixture.detectChanges();
    let stories: NodeList =
      fixture.nativeElement.querySelectorAll('app-news-story');
    expect(stories.length).toBe((fakeLatestNews()?.page ?? []).length);
  });
});
