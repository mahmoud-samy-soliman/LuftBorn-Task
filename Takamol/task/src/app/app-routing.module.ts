import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientEditComponent } from './client-edit/client-edit.component';
import { ClientListComponent } from './client-list/client-list.component';
import { ClientRegisterComponent } from './client-register/client-register.component';
import { LoginComponent } from './login/login/login.component';
const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'clients', component: ClientListComponent },
  { path: 'clients/create', component: ClientRegisterComponent },
  { path: 'clients/edit/:id', component: ClientEditComponent },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
