import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditEmployeeComponent } from './add-employee.component';

describe('AddEmployeeComponent', () => {
  let component: AddEditEmployeeComponent;
  let fixture: ComponentFixture<AddEditEmployeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddEditEmployeeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddEditEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
