import { Component, OnInit } from '@angular/core';
import { EmployeesService } from 'src/app/shared/services/employee/employees.service';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.scss']
})
export class AddEmployeeComponent implements OnInit {

  constructor(private service:EmployeesService) { }

  ngOnInit(): void {
  }

}
