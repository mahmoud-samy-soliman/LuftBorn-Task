import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl , FormGroup , Validators , ReactiveFormsModule } from '@angular/forms';
import{Router} from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import{AuthService} from 'src/app/service/auth.service'
import { Login } from 'src/app/models/login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  login:Login={} as Login;
  errorMessage:string="";
  Token:any = {};
  loginForm:FormGroup ;
  

  constructor(private fb:FormBuilder,private loginService:AuthService,private router : Router){
    this.loginForm=this.fb.group({
      email : new FormControl(null , [
        Validators.required , Validators.email
      ]),
      password : new FormControl(null , [
      Validators.required
      ]),
      rememberMe: new FormControl(false)
    });

  }

  get email(){
    return this.loginForm.get('email');
  }
  get password(){
    return this.loginForm.get('password');
  }
  get rememberMe(){
    return this.loginForm.get('rememberMe');
  }
  ngOnInit(): void {
  }

  submitLoginForm(): void {
    this.login.password=this.password?.value
    this.login.email=this.email?.value
    this.login.rememberMe=this.rememberMe?.value
    this.loginService.Login(this.login).subscribe({
      next: (data) => {
        this.Token=data

        localStorage.setItem('userToken' , this.Token.token);
        //console.log(data);
        this.loginService.saveUserData();
        const token = localStorage.getItem('userToken');
        if (token) {
          const decodedToken: any = jwtDecode(token);
          this.router.navigate(['/clients']);

        } 
      },
      error: (err) => this.errorMessage = err
    });
  }
}
