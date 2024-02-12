import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Client } from '../models/client';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ClientService {
  private apiUrl = 'https://localhost:7124/api/Client'; 
  private registerUrl = `https://localhost:7124/api/Client/create`

  constructor(private http: HttpClient, private userService: AuthService) { } 

  getClients(): Observable<Client[]> {
    return this.http.get<Client[]>(this.apiUrl);
  }

  createClient(client: Client): Observable<void> {
    const createdById = this.userService.getCurrentUserId() ?? 'defaultUserId';
    client.createdById = createdById;
    client.creationDate = new Date();
    return this.http.post<void>(this.registerUrl , client);
  }

  updateClient(id: string, client: Client): Observable<void> {
    const modifiedById = this.userService.getCurrentUserId() ?? 'defaultUserId';
    client.modifiedById = modifiedById;
    client.modificationDate = new Date();
    const url = `${this.apiUrl}/update/${id}`;
    return this.http.put<void>(url, client);
  }

  deleteClient(id: string): Observable<void> {
    const url = `${this.apiUrl}/delete/${id}`;
    return this.http.delete<void>(url);
  }
}
