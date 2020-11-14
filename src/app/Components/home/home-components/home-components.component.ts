import { Component, OnInit } from '@angular/core';
import {ConfigServices} from '../../../services/config/config-services';
@Component({
  selector: 'app-home-components',
  templateUrl: './home-components.component.html',
  styleUrls: ['./home-components.component.scss']
})
export class HomeComponentsComponent implements OnInit {
   slides = [{'image': '../../../../assets/IICT.jpg'}, {'image': '../../../../assets/IICT.jpg'},{'image': '../../../../assets/IICT.jpg'}, {'image': 'https://gsr.dev/material2-carousel/assets/demo.png'}, {'image': 'https://gsr.dev/material2-carousel/assets/demo.png'}];

  constructor(private configServices : ConfigServices ) { }

  ngOnInit(): void {
    this.configServices.getValues();
  }


}
