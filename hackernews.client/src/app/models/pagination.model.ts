export interface Pagination {
  pageNumber: number;
  pageSize: number;
}

export interface PaginationWithSearch extends Pagination {
  searchString: string;
}
