import { UserRoleDto } from "./UserRoleDto";

export class UserProfileDto{
    id:string;
    email:string;
    contact: string;
    role: UserRoleDto;
    personalEmail: string;
    dateOfJoining: Date;

    constructor(id:string,email:string,contact:string,role:UserRoleDto,personalEmail:string,dateOfJoining:Date){
        this.id = id;
        this.email = email;
        this.contact = contact;
        this.role = role;
        this.personalEmail = personalEmail;
        this.dateOfJoining = dateOfJoining;
    }
}