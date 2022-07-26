export class UserRoleDto
{
    id: string;
    title: string;
    description: string;

    constructor(id:string,title:string,description:string)
    {
        this.id = id;
        this.title = title;
        this.description = description;
    }
}

export class ApplicationRolesDto 
{
    id: string;
    name: string;

    constructor(id:string,name:string){
        this.id = id;
        this.name = name;
    }
}