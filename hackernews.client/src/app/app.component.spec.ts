import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RouterOutlet } from '@angular/router';
import { dispatch, provideStore, Store } from '@ngxs/store';
import { CUSTOM_ELEMENTS_SCHEMA, Signal, signal } from '@angular/core';
import { NewsState } from './store/news/news.state';
import { of } from 'rxjs';
import { By } from '@angular/platform-browser';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let store: Store;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [provideStore([])],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    store = TestBed.inject(Store);
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should apply dark theme is state theme is dark', () => {
    const dark: Signal<string> = signal('dark');
    spyOn(store, 'selectSignal').and.returnValue(dark);
    component.ngOnInit();
    fixture.detectChanges();
    const template = fixture.nativeElement.querySelector('div.dark-mode');
    expect(component.theme()).toBe('dark-mode');
    expect(template).not.toBeNull();
  });

  it('should apply light theme is state theme is light', () => {
    const light: Signal<string> = signal('light');
    spyOn(store, 'selectSignal').and.returnValue(light);
    component.ngOnInit();
    fixture.detectChanges();
    const template = fixture.nativeElement.querySelector('div.light-mode');
    expect(component.theme()).toBe('light-mode');
    expect(template).not.toBeNull();
  });
});
