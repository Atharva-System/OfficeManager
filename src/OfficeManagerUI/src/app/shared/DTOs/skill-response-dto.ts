export class SkillResponseDto
{
  id: number;
  name: string;
  description: string;
  isActive: boolean;

  constructor(){
    this.id = 0;
    this.name = "";
    this.description = "";
    this.isActive = true;
  }
}

export interface ISkillResponseDto {
  id: number;
  name: string;
  description: string;
  isActive: boolean;
}

export interface ISkillLevel {
  id: number;
  name: string;
  description: string;
  isActive: boolean;
}

export class SkillLevel implements ISkillLevel {
  id: number;
  name: string;
  description: string;
  isActive: boolean;

  constructor() {
    this.id = 0;
    this.name = "";
    this.description = "";
    this.isActive = true;
  }
}



export interface ISkillRate {
  id: number;
  name: string;
  description: string;
  isActive: boolean;
}

export class SkillRate implements ISkillRate {
  id: number;
  name: string;
  description: string;
  isActive: boolean;

  constructor() {
    this.id = 0;
    this.name = "";
    this.description = "";
    this.isActive = true;
  }
}
