import { TestBed } from '@angular/core/testing';
import { provideStore, Store } from '@ngxs/store';
import { NewsState, NewsStateModel } from './news.state';

describe('News store', () => {
  let store: Store;
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideStore([NewsState])],
    });

    store = TestBed.inject(Store);
  });

  // it('should create an action and add an item', () => {
  //   const expected: NewsStateModel = {
  //     items: ['item-1']
  //   };
  //   store.dispatch(new NewsAction('item-1'));
  //   const actual = store.selectSnapshot(NewsState.getState);
  //   expect(actual).toEqual(expected);
  // });
});
