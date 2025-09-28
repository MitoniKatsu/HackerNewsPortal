import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PagedResponse } from '../models/paged-response.model';
import { NewsStory } from '../models/news-story.model';
import { RankedNewsStory } from '../models/ranked-news-story.model';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  constructor(private http: HttpClient) {}

  baseUrl = '';

  getLatestNews(pageNumber: number, pageSize: number) {
    return this.http.get<PagedResponse<NewsStory>>(
      `${this.baseUrl}/api/news?pagenumber=${pageNumber}&pagesize=${pageSize}`,
      { observe: 'response' }
    );
  }
  getRankedNews(pageNumber: number, pageSize: number, searchString: string) {
    return this.http.get<PagedResponse<RankedNewsStory>>(
      `${this.baseUrl}/api/news/search?searchString=${searchString}&pagenumber=${pageNumber}&pagesize=${pageSize}`,
      { observe: 'response' }
    );
  }
}
