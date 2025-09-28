import { TestBed } from '@angular/core/testing';

import { NewsService } from './news.service';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { HttpResponse, provideHttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { PagedResponse } from '../models/paged-response.model';
import { NewsStory } from '../models/news-story.model';
import { RankedNewsStory } from '../models/ranked-news-story.model';

describe('NewsService', () => {
  let service: NewsService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), NewsService],
    });
    service = TestBed.inject(NewsService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    http.verify();
  });

  it('should be created', () => {
    service = TestBed.inject(NewsService);
    expect(service).toBeTruthy();
  });

  it('should request API for latest news when getLatestNews is called', (done) => {
    const res = new PagedResponse<NewsStory>();
    service.getLatestNews(1, 10).subscribe((response) => {
      expect(response.body).toEqual(res);
      done();
    });
    const req = http.expectOne(
      `${service.baseUrl}/api/news?pagenumber=1&pagesize=10`
    );
    expect(req.request.method).toBe('GET');
    req.flush(res);
    http.verify();
  });

  it('should request API for ranked news when getRankedNews is called', (done) => {
    const res = new PagedResponse<RankedNewsStory>();
    service.getRankedNews(1, 10, 'test').subscribe((response) => {
      expect(response.body).toEqual(res);
      done();
    });
    const req = http.expectOne(
      `${service.baseUrl}/api/news/search?searchString=test&pagenumber=1&pagesize=10`
    );
    expect(req.request.method).toBe('GET');
    req.flush(res);
    http.verify();
  });
});
