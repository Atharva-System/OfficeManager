import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { UserRoleDto } from 'src/app/shared/Authentication/dtos/UserRoleDto';
import { AddroleComponent } from './addrole/addrole.component';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {

  constructor(private service:AuthenticationservicesService,public dialog: MatDialog) { }
  displayedColumns: string[] = ['no','title','description','action']
  dataSource:MatTableDataSource<UserRoleDto> = new MatTableDataSource<UserRoleDto>();

  ngOnInit(): void {
    this.service.getUserRoles();
    //this.dataSource = new MatTableDataSource<UserRoleDto>(this.service.roles);
    this.loadData();
  }

  loadData()
  {
    this.service.role$.subscribe((roles:UserRoleDto[])=>
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
        title: '',
        description: ''
      }
    })
    dialogRef.afterClosed().subscribe(result => {
      this.loadData();
    })
  }

  editRole(role:UserRoleDto) : void
  {
    const dialogRef = this.dialog.open(AddroleComponent,{
      width: '500px',
      data:{
        id: role.id,
        title: role.title,
        description: role.description
      }
    })
    dialogRef.afterClosed().subscribe(result => {
      this.loadData();
    })
  }

  deleteRole(id:string)
  {
    if(confirm("do you realy want to delete the role"))
    {
      if(this.service.deleteRole(id))
      {
        this.loadData();
      }
    }
  }

}
