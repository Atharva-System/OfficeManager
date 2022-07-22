export class WeatherForecastDto {
    date: Date;
    temperatureC: number;
    temperatureF: number;
    summary: string;

    constructor(date:Date,temperatureC:number,temperatureF:number,summary:string){
        this.date = date;
        this.temperatureC = temperatureC;
        this.temperatureF = temperatureF;
        this.summary = summary;
    }
}