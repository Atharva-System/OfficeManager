import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { DesignationDto } from 'src/app/shared/Authentication/dtos/DesignationDto';
import { DesignationService } from 'src/app/shared/Services/designation.service';
import { CreateComponent } from './create/create.component';

@Component({
  selector: 'app-designations',
  templateUrl: './designations.component.html',
  styleUrls: ['./designations.component.scss']
})
export class DesignationsComponent implements OnInit {

  displayedColumns:string[] = ['id','name','actions'];
  dataSource:MatTableDataSource<DesignationDto> = new MatTableDataSource<DesignationDto>();

  constructor(private service:DesignationService,private dialog:MatDialog) { }

  ngOnInit(): void {
    this.service.getDesignations('');
    this.loadData();
  }

  loadData(): void{
    this.service.designationList$.subscribe((result:DesignationDto[]) =>{
      this.dataSource.data = result;
    });
  }

  addDesignation(): void{
    this.dialog.open(CreateComponent,
      {
        width: '500px',
        data:{
          id:'',
          name:''
        }
      })
  }

  deleteDesignation(id:string): void{
    if(confirm("Do you realy want to delete this designation?"))
    {
      this.service.deleteDesignation(id);
    }
  }

}
