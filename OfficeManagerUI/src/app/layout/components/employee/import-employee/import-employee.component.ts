import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BIEmployeeResponseDto } from 'src/app/shared/DTOs/bi-employee-response-dto';
import { DepartmentResponseDto } from 'src/app/shared/DTOs/department-response-dto';
import { DesignationResponseDto } from 'src/app/shared/DTOs/designation-response-dto';

@Component({
  selector: 'app-import-employee',
  templateUrl: './import-employee.component.html',
  styleUrls: ['./import-employee.component.scss']
})
export class ImportEmployeeComponent implements OnInit {

  @Input() employees:BIEmployeeResponseDto[] = [];
  @Input() departments: DepartmentResponseDto[] = [];
  @Input() designations: DesignationResponseDto[] =[];
  @Input() Loading$:Observable<boolean> = new Observable<boolean>();

  constructor() { }

  ngOnInit(): void {
  }

}
