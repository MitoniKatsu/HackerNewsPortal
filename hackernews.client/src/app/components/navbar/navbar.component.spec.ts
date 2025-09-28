import {
  ComponentFixture,
  fakeAsync,
  TestBed,
  tick,
} from '@angular/core/testing';

import { NavbarComponent } from './navbar.component';
import { provideStore } from '@ngxs/store';
import { provideRouter, Router } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

describe('NavbarComponent', () => {
  let component: NavbarComponent;
  let fixture: ComponentFixture<NavbarComponent>;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarComponent],
      providers: [provideStore(), provideRouter([])],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NavbarComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate to home when menu clicked', fakeAsync(() => {
    spyOn(router, 'navigate');

    let button = fixture.nativeElement.querySelector('#btnMenu');
    button.click();
    tick();
    expect(router.navigate).toHaveBeenCalledOnceWith(['home']);
  }));

  it('should navigate to home when Top Stories clicked', fakeAsync(() => {
    spyOn(router, 'navigate');

    let button = fixture.nativeElement.querySelector('#btnTopStories');
    button.click();
    tick();
    expect(router.navigate).toHaveBeenCalledOnceWith(['home']);
  }));

  it('should navigate to home when Search clicked', fakeAsync(() => {
    spyOn(router, 'navigate');

    let button = fixture.nativeElement.querySelector('#btnSearch');
    button.click();
    tick();
    expect(router.navigate).toHaveBeenCalledOnceWith(['search']);
  }));
});
