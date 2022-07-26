import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { ApplicationRolesDto, UserRoleDto } from 'src/app/shared/Authentication/dtos/UserRoleDto';
import { AddroleComponent } from './addrole/addrole.component';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {

  constructor(private service:AuthenticationservicesService,public dialog: MatDialog) { }
  displayedColumns: string[] = ['no','title','action']
  dataSource:MatTableDataSource<ApplicationRolesDto> = new MatTableDataSource<ApplicationRolesDto>();

  ngOnInit(): void {
    this.service.getApplicationUserRoles();
    //this.dataSource = new MatTableDataSource<UserRoleDto>(this.service.roles);
    this.loadData();
  }

  loadData()
  {
    this.service.appRoles$.subscribe((roles:ApplicationRolesDto[])=>
    {
      this.dataSource.data = roles;
    })
  }

  addRole() : void
  {
    const dialogRef = this.dialog.open(AddroleComponent,{
      width: '500px',
      data:{
        id: '',
        title: ''
      }
    })
  }

  deleteRole(id:string)
  {
    if(confirm("do you realy want to delete the role"))
    {
      this.service.deleteRole(id);
    }
  }

}
