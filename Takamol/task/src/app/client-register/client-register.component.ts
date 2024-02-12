import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ClientService } from '../service/client.service';
import { Client } from '../models/client';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

enum Gander {
  Male = 1,
  Female = 2,
}
@Component({
  selector: 'app-client-register',
  templateUrl: './client-register.component.html',
  styleUrls: ['./client-register.component.css']
})

export class ClientRegisterComponent implements OnInit {
  registeruser:Client={} as Client;
  errorMessage:string="";
  
  
  registerForm:FormGroup ;
  

  constructor(private fb:FormBuilder,private registertion:ClientService,private router : Router){
    this.registerForm=this.fb.group({
      name : new FormControl(null , [
        Validators.required , Validators.pattern(`^[A-Za-z]*$`)
      ]),
      address : new FormControl(null , [
        Validators.required 
      ]),
      description : new FormControl(null , [
        Validators.required 
      ]),
      jobTitle : new FormControl(null , [
        Validators.required 
      ]),
      clientClassification : new FormControl(null , [
        Validators.required 
      ]),
      clientSource : new FormControl(null , [
        Validators.required 
      ]),
      salesMan : new FormControl(null , [
        Validators.required 
      ])
    });

  }

  get name(){
    return this.registerForm.get('name');
  }
  get address(){
    return this.registerForm.get('address');
  }
  get description() {
    return this.registerForm.get('description');
  }
   
  get jobTitle(){
    return this.registerForm.get('jobTitle');
  }
  get clientClassification(){
    return this.registerForm.get('clientClassification');
  }
  get clientSource(){
    return this.registerForm.get('clientSource');
  }
  get salesMan(){
    return this.registerForm.get('salesMan');
  }

  ngOnInit(): void {
  }

  registerData(): void {
    this.registeruser.name=this.name?.value
    this.registeruser.jobTitle=this.jobTitle?.value
    this.registeruser.description=this.description?.value
    this.registeruser.clientSource=this.clientSource?.value
    this.registeruser.clientClassification = this.clientClassification?.value
    this.registeruser.salesMan = this.salesMan?.value
    this.registeruser.address = this.address?.value;
    console.log(this.registeruser);
    this.registertion.createClient(this.registeruser).subscribe({
      next: (data) => {
        console.log(data);
        // Redirect to /students route
        this.router.navigate(['/clients']);
      },
      error: (err) => this.errorMessage = err
    });
  }
}

