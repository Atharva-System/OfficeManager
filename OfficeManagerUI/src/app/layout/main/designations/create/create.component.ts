import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DesignationDto } from 'src/app/shared/Authentication/dtos/DesignationDto';
import { DesignationService } from 'src/app/shared/Services/designation.service';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent implements OnInit {

  constructor(public dialogRef:MatDialogRef<CreateComponent>,@Inject(MAT_DIALOG_DATA) public data:DesignationDto,private service:DesignationService) { }

  ngOnInit(): void {
  }

  saveDesignation(): void{
    this.service.addDesignation(this.data);
    this.dialogRef.close();
  }

}
