import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserpostDetailsComponent } from './userpost-details.component';

describe('UserpostDetailsComponent', () => {
  let component: UserpostDetailsComponent;
  let fixture: ComponentFixture<UserpostDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserpostDetailsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UserpostDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
