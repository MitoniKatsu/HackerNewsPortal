import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  constructor(private http: HttpClient) {}

  getLatestNews(pageNumber: number, pageSize: number) {
    return this.http.get(
      `https://localhost:7278/api/news?pagenumber=${pageNumber}&pagesize=${pageSize}`,
      { observe: 'response' }
    );
  }
  getRankedNews(pageNumber: number, pageSize: number, searchString: string) {
    return this.http.get(
      `https://localhost:7278/api/news/search?searchString=${searchString}&pagenumber=${pageNumber}&pagesize=${pageSize}`,
      { observe: 'response' }
    );
  }
}
