import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserPaymentRequestComponent } from './user-payment-request.component';

describe('UserPaymentRequestComponent', () => {
  let component: UserPaymentRequestComponent;
  let fixture: ComponentFixture<UserPaymentRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserPaymentRequestComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserPaymentRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
