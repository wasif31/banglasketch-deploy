import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ConfigServices implements OnInit{
  values:any;
  constructor(private http: HttpClient) {

   }
  ngOnInit(){

  }
  getValues(){
    this.http.get('https://localhost:5001/weatherforecast').subscribe(x=>{
      this.values=x;
      console.log(this.values);
    },
    error=>{
      console.log(error);
    });
  }

}
