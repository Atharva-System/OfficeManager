export class PaginatedResponse<T>
{
  items:Array<T>;
  pageNumber: number;
  totalPages: number;
  totalCount: number;

  constructor()
  {
    this.items = [];
    this.pageNumber = 1;
    this.totalPages = 1;
    this.totalCount = 1;
  }
}

export interface IPaginatedResponse<T>{
  items:Array<T>;
  pageNumber: number;
  totalPages: number;
  totalCount: number;
}
