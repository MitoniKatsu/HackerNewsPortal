import {
  Pagination,
  PaginationWithSearch,
} from '../../models/pagination.model';

export namespace NewsActions {
  export class GetLatestNews {
    static readonly type = '[News] GetLatestNews';
    constructor(readonly payload: Pagination) {}
  }
  export class GetRankedNews {
    static readonly type = '[News] GetRankedNews';
    constructor(readonly payload: PaginationWithSearch) {}
  }
  export class ResetSearch {
    static readonly type = '[News] ResetSearch';
  }
  export class ToggleTheme {
    static readonly type = '[News] ToggleTheme';
  }
}
