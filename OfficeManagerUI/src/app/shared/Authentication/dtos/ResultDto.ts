export class ResultDto {
    succeeded: boolean;
    errors: string[];
    message: string;
    constructor(succeedd:boolean,errors:string[],message:string){
        this.succeeded = succeedd;
        this.errors = errors;
        this.message = message;
    }
}