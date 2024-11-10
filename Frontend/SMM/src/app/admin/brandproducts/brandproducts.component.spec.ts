import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BrandproductsComponent } from './brandproducts.component';

describe('BrandproductsComponent', () => {
  let component: BrandproductsComponent;
  let fixture: ComponentFixture<BrandproductsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BrandproductsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BrandproductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
