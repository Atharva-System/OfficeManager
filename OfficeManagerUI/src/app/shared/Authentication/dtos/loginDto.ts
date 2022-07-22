export class LoginDto {
    Email: string;
    Password: string;

    constructor(email:string,password:string){
        this.Email = email;
        this.Password =  password;
    }
}

export class loginResponseDto {
    userId: string;
    email: string;
    contact: string;
    token: string;
    role: string;
    constructor(userId:string,email:string,contact:string,token:string,role:string){
        this.userId = userId;
        this.email = email;
        this.contact = contact;
        this.token = token;
        this.role = role;
    }
}