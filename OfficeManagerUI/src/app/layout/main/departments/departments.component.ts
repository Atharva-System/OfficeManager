import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { DepartmentDto } from 'src/app/shared/Authentication/dtos/DepartmentDto';
import { DepartmentService } from 'src/app/shared/Services/department.service';
import { CreateComponent } from './create/create.component';

@Component({
  selector: 'app-departments',
  templateUrl: './departments.component.html',
  styleUrls: ['./departments.component.scss']
})
export class DepartmentsComponent implements OnInit {

  displayedColumns:string[] = ['id','name','description','action']
  dataSource:MatTableDataSource<DepartmentDto> = new MatTableDataSource<DepartmentDto>();

  constructor(private service:DepartmentService,public dialog: MatDialog) { }

  ngOnInit(): void {
    this.service.getDepartments('');
    this.loadData();
  }

  loadData():void {
    this.service.departmentList$.subscribe((result:DepartmentDto[])=>{
      this.dataSource.data = result;
    })
  }

  addDepartment(){
    const dialogRef = this.dialog.open(CreateComponent,{
      width: '500px',
      data:{
        id: '',
        title: '',
        description:''
      }
    })
  }

  editDepartment(data:DepartmentDto):void {
    const dialogRef = this.dialog.open(CreateComponent,{
      width: '500px',
      data:data
    });
  }

  deleteDepartment(id:string): void {
    if(confirm("Do you realy want to delete this department?")){
      this.service.deleteDepartment(id);
    }
  }

}
