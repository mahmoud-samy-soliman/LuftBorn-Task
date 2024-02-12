import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ClientService } from '../service/client.service';
import { Client } from '../models/client';

@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css']
})
export class ClientListComponent implements OnInit {
  clients: Client[] = [];
  dataSource: MatTableDataSource<Client>;

  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;

  constructor(private router: Router, private clientService: ClientService) {
    this.dataSource = new MatTableDataSource<Client>(this.clients);
  }

  ngOnInit(): void {
    this.loadClients();
  }

  loadClients() {
    this.clientService.getClients().subscribe(data => {
      this.clients = data;
      this.dataSource.data = this.clients;
      this.dataSource.paginator = this.paginator;
    });
  }

  resetTable() {
    this.loadClients();
  }

  deleteClient(id: string): void {
    this.clientService.deleteClient(id)
      .subscribe(() => {
        this.clients = this.clients.filter(client => client.id !== id);
        this.dataSource.data = this.clients;
      });
  }

  editClient(id: string): void {
    this.router.navigateByUrl('/clients/edit/' + id);
  }
}
