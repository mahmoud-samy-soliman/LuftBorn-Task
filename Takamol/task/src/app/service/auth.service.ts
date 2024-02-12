import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Login } from '../models/login';
import { HttpClient } from '@angular/common/http';
import { Token } from '@angular/compiler';
import  { jwtDecode } from 'jwt-decode';
import{Router} from '@angular/router'
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loginUrl = 'https://localhost:7124/api/Account';
  userData = new BehaviorSubject<any>(null);

  constructor(private http : HttpClient , private router : Router) { 
    if(localStorage.getItem('userToken')!=null){
      this.saveUserData();
    }
  }
  Login(login: Login): Observable<Token> {
    const url = `${this.loginUrl}/Login`;
    return this.http.post<Token>(url , login);
  }

  saveUserData() {
    const userToken = localStorage.getItem('userToken');
    if (userToken) {
      const decodedUserData = jwtDecode(userToken);
      this.userData.next(decodedUserData);
    }
  }
  getCurrentUserId(): string | null {
    const userData = this.userData.getValue();
    const firstElement = userData['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'][0];
    return userData ? firstElement : null;
  }
  getUserId(): Observable<string | null> {
    return this.userData.asObservable();
  }

  Logout (){
    localStorage.removeItem('userToken');
    this.userData.next(null);
    this.router.navigate(['/login']);
  }
}
