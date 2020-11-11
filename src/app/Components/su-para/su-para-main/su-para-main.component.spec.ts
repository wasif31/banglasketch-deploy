import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuParaComponent } from './su-para-main.component';

describe('SuParaComponent', () => {
  let component: SuParaComponent;
  let fixture: ComponentFixture<SuParaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SuParaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SuParaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
