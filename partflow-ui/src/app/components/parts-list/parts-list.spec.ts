import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PartsList } from './parts-list';

describe('PartsList', () => {
  let component: PartsList;
  let fixture: ComponentFixture<PartsList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PartsList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PartsList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
