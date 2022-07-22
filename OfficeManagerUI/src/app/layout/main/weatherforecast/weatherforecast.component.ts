import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { AuthenticationservicesService } from 'src/app/shared/Authentication/authenticationservices.service';
import { WeatherForecastDto } from 'src/app/shared/Authentication/dtos/WeatherForecastDto';

@Component({
  selector: 'app-weatherforecast',
  templateUrl: './weatherforecast.component.html',
  styleUrls: ['./weatherforecast.component.scss']
})
export class WeatherforecastComponent implements OnInit {

  constructor(private service:AuthenticationservicesService) { }
  displayedColumns: string[] = ['date','temperatureC','temperatureF','summary']
  dataSource:MatTableDataSource<WeatherForecastDto> = new MatTableDataSource<WeatherForecastDto>();

  ngOnInit(): void {
    this.service.getWeatherForecast();
    this.service.weatherForecast$.subscribe((result:WeatherForecastDto[])=>{
      this.dataSource.data = result;
    })
  }

}
