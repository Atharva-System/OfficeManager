export class LoginResponseDto {
  userId: number;
  employeeNo: number;
  email: string;
  token: string;
  constructor()
  {
    this.userId = 0;
    this.employeeNo = 0;
    this.email = "";
    this.token = "";
  }
}
