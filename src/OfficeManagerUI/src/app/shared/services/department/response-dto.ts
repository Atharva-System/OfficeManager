export class ResponseDto {
  _Message: string;
  _Data: any;
  _StatusCode: string;
  _Errors: string[];
  constructor()
  {
    this._Data = [];
    this._Errors = [];
    this._StatusCode = "200";
    this._Message = "";
  }
}
