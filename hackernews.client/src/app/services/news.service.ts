import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  constructor(private http: HttpClient) {}

  baseUrl = 'https://localhost:7278';

  getLatestNews(pageNumber: number, pageSize: number) {
    return this.http.get(
      `${this.baseUrl}/api/news?pagenumber=${pageNumber}&pagesize=${pageSize}`,
      { observe: 'response' }
    );
  }
  getRankedNews(pageNumber: number, pageSize: number, searchString: string) {
    return this.http.get(
      `${this.baseUrl}/api/news/search?searchString=${searchString}&pagenumber=${pageNumber}&pagesize=${pageSize}`,
      { observe: 'response' }
    );
  }
}
