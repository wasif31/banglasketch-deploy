import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {
  Roles: any = ['Student', 'Teacher', 'Visitor'];
  selectedRoles:any;
  selectedType:any;
  form: FormGroup;

  constructor() { }

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.maxLength(20)]),
      regNumber: new FormControl('', [Validators.required, Validators.maxLength(10),Validators.pattern("^[0-9]*$"),]),
      email:new FormControl('', [Validators.required, Validators.pattern("^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")]),
      password:new FormControl('', [Validators.required])
    });
  }
  public hasError = (controlName: string, errorName: string) =>{
    return this.form.controls[controlName].hasError(errorName);
  }
  selectedRolesChange($event: any)
{
  this.selectedRoles =  $event;

}

}
