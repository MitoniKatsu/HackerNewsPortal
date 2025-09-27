export class PagedResponse<T> {
  total?: number;
  pageNumber?: number;
  pageCount?: number;
  page?: T[];
}
