import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ThemeToggleComponent } from './theme-toggle.component';
import { provideStore, Store } from '@ngxs/store';
import { NewsActions } from '../../store/news/news.actions';
import { By } from '@angular/platform-browser';

describe('ThemeToggleComponent', () => {
  let component: ThemeToggleComponent;
  let fixture: ComponentFixture<ThemeToggleComponent>;
  let store: Store;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ThemeToggleComponent, MatSlideToggleModule],
      providers: [provideStore([])],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ThemeToggleComponent);
    component = fixture.componentInstance;
    store = TestBed.inject(Store);
    spyOn(store, 'dispatch').and.callThrough();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should dispatch toggleTheme action when toggle changed', fakeAsync(() => {
    let toggle = fixture.debugElement.query(By.css('mat-slide-toggle'));
    toggle.triggerEventHandler('change', null);
    tick();

    expect(store.dispatch).toHaveBeenCalledOnceWith(
      new NewsActions.ToggleTheme()
    );
  }));
});
