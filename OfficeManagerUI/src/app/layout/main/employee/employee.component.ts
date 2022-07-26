import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddEditFormComponent } from './add-edit-form/add-edit-form.component';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss']
})
export class EmployeeComponent implements OnInit {

  constructor(private dialog:MatDialog) { }

  ngOnInit(): void {
  }

  addEmployee():void {
    const dialogRef = this.dialog.open(AddEditFormComponent,{
      width: '500px',
      height: 'fit-content'
    })
    
    dialogRef.afterClosed().subscribe(result => {
      console.log("added");
    })
  }

}
