export class LoginDto {
    UserName: string;
    Password: string;

    constructor(userName:string,password:string){
        this.UserName = userName;
        this.Password =  password;
    }
}

export class loginResponseDto {
    userId: string;
    email: string;
    token: string;
    role: string[];
    constructor(userId:string,email:string,contact:string,token:string,role:string[]){
        this.userId = userId;
        this.email = email;
        this.token = token;
        this.role = role;
    }
}