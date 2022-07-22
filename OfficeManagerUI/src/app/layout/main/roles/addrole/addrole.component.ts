import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { UserRoleDto } from 'src/app/shared/Authentication/dtos/UserRoleDto';

@Component({
  selector: 'app-addrole',
  templateUrl: './addrole.component.html',
  styleUrls: ['./addrole.component.scss']
})
export class AddroleComponent implements OnInit {

  constructor(public dialogRef:MatDialogRef<AddroleComponent>,@Inject(MAT_DIALOG_DATA) public data:UserRoleDto,private service:AuthenticationservicesService) { }

  ngOnInit(): void {
  }

  saveRole(){
    if(this.data.id == '')
    {
      this.service.addRole(this.data)
    }
    else{
      this.service.editRole(this.data);
    }
    this.dialogRef.close();
  }

}
