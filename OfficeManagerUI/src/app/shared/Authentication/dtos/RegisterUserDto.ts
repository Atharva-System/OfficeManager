export class RegisterUserDto
{
    Email:string;
    Password:string;
    Contact:string;
    PersonalEmail:string;
    DateOfJoining:Date;
    RoleId:string;

    constructor(email:string,password:string,contact:string,personalEmail:string,dateOfJoining:Date,roleId:string)
    {
        this.Email = email;
        this.Password = password;
        this.Contact = contact;
        this.DateOfJoining = dateOfJoining;
        this.PersonalEmail = personalEmail;
        this.RoleId = roleId;
    }
}