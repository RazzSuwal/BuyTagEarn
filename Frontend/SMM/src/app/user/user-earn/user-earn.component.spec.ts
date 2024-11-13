import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserEarnComponent } from './user-earn.component';

describe('UserEarnComponent', () => {
  let component: UserEarnComponent;
  let fixture: ComponentFixture<UserEarnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserEarnComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserEarnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
