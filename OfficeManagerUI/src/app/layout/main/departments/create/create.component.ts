import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DepartmentDto } from 'src/app/shared/Authentication/dtos/DepartmentDto';
import { DepartmentService } from 'src/app/shared/Services/department.service';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent implements OnInit {

  constructor(public dialogRef:MatDialogRef<CreateComponent>,@Inject(MAT_DIALOG_DATA) public data:DepartmentDto,private service:DepartmentService) { }

  ngOnInit(): void {
  }

  saveDepartment():void {
    if(this.data.id === '')
      this.service.addDepartment(this.data);
    else
      this.service.editDepartment(this.data);
    this.dialogRef.close();
  }

}
