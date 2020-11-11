import { Component, OnInit } from '@angular/core';
import {FormGroup, FormControl, Validators} from '@angular/forms';


@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {

  addPeopleForm: FormGroup;
  addNewsForm: FormGroup;
  addPublicationForm: FormGroup;

  ngOnInit(): void {
   this.initAddPeopleForm();
   this.initAddPublicationForm();
   this.initAddNewsForm();
  }

  initAddPeopleForm() {
    this.addPeopleForm = new FormGroup({
      name: new FormControl('', Validators.required),
      designation: new FormControl('', Validators.required),
      affiliation: new FormControl('', Validators.required),
      researchInterest: new FormControl('', Validators.required),
      joiningYear: new FormControl('', Validators.required),
      personalWebsiteLink: new FormControl(''),
      linkedinProfile: new FormControl(''),
      shortDescription: new FormControl(''),
    });
  }

  initAddPublicationForm() {
    this.addPublicationForm = new FormGroup({
      publicationTitle: new FormControl('', Validators.required),
      authors: new FormControl('', Validators.required),
      publicationYear: new FormControl('', Validators.required),
      publicationType: new FormControl('', Validators.required),
      publicationLink: new FormControl('', Validators.required),
      conferenceName: new FormControl('', Validators.required),
    });
  }

  initAddNewsForm() {
    this.addNewsForm = new FormGroup({
      newsTitle: new FormControl('', Validators.required),
      headline: new FormControl('', Validators.required),
      newsBody: new FormControl('', Validators.required),
    });
  }

  addPeopleSubmit() {
    console.log(this.addPeopleForm.value);
  }

  addNewsSubmit() {
    console.log(this.addNewsForm.value);
  }

  addPublicationSubmit() {
    console.log(this.addPublicationForm.value);
  }

}
