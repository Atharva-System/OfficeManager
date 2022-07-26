export class RegisterEmployeeDto {
    FirstName: string;
    LastName: string;
    Email: string;
    Username: string;
    Password: string;
    roleId: string;
    PersonalEmail: string;
    PhoneNumber: string;
    PersonalPhoneNumber: string;
    DateOfJoining: Date;
    DateOfBirth: Date;
    DepartmentId: string;
    DesignationId: string;

    constructor(firstName:string,lastName:string,email:string,username:string,password:string,roleId:string,personalEmail:string,phoneNumber:string,
        personalPhoneNumber:string,dateOfJoining:Date,dateOfBirth:Date,departmentId:string,designationId:string)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Username = username;
            this.Password = password;
            this.roleId = roleId;
            this.PersonalEmail = personalEmail;
            this.PhoneNumber = phoneNumber;
            this.PersonalPhoneNumber = personalPhoneNumber;
            this.DateOfJoining = dateOfJoining;
            this.DateOfBirth = dateOfBirth;
            this.DepartmentId = departmentId;
            this.DesignationId = designationId;
        }
    
}