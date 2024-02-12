import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientService } from '../service/client.service';
import { Client } from '../models/client';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-client-edit',
  templateUrl: './client-edit.component.html',
  styleUrls: ['./client-edit.component.css']
})
export class ClientEditComponent implements OnInit{
  registeruser:Client={} as Client;
  errorMessage:string="";
  id:String = "";
  registerForm:FormGroup ;
  
  
  constructor(private fb:FormBuilder,private registertion:ClientService,private router : Router , private activatedRoute:ActivatedRoute){
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
    this.activatedRoute.paramMap.subscribe(map=>{
      this.id = map.get("id")?? "1";
    })
  }

  registerData(): void {
    this.registeruser.name=this.name?.value
    this.registeruser.jobTitle=this.jobTitle?.value
    this.registeruser.description=this.description?.value
    this.registeruser.clientSource=this.clientSource?.value
    this.registeruser.clientClassification = this.clientClassification?.value
    this.registeruser.salesMan = this.salesMan?.value
    this.registeruser.address = this.address?.value;
    this.registeruser.id = this.id?.valueOf();
    console.log(this.registeruser);
    this.registertion.updateClient(this.registeruser.id, this.registeruser).subscribe({
      next: (data) => {
        console.log(data);
        // Redirect to /clients route
        this.router.navigate(['/clients']);
      },
      error: (err) => this.errorMessage = err
    });
  }

  editClient(): void {
    this.registertion.updateClient(this.registeruser.id,this.registeruser).subscribe(
      () => {
        console.log('client updated successfully.');
        // Redirect to /students route
        this.router.navigate(['/clients']);
      },
      (error) => {
        this.errorMessage = error;
        console.error('Error updating student:', error);
      }
    );
  }
}
