import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-side-navbar',
  templateUrl: './side-navbar.component.html',
  styleUrls: ['./side-navbar.component.scss']
})
export class SideNavbarComponent implements OnInit {
@Output() closeSidenav = new EventEmitter<void>();
  constructor() { }

  ngOnInit(): void {
  }
  onClose() {
    this.closeSidenav.emit();
  }


}
