import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsStoryComponent } from './news-story.component';
import { ComponentRef, signal } from '@angular/core';

describe('NewsStoryComponent', () => {
  let component: NewsStoryComponent;
  let componentRef: ComponentRef<NewsStoryComponent>;
  let fixture: ComponentFixture<NewsStoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewsStoryComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(NewsStoryComponent);
    component = fixture.componentInstance;
    componentRef = fixture.componentRef;
    componentRef.setInput('title', 'testingTitle');
    componentRef.setInput('url', 'testingUrl');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should contain title inside the span tag', () => {
    let span = fixture.nativeElement.querySelector('span');
    expect(span.innerText).toBe('testingTitle');
  });

  it('should contain url inside the a tag', () => {
    let a = fixture.nativeElement.querySelector('a');
    expect(a.innerText).toBe('testingUrl');
  });
});
