import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { AllAccountsComponent } from './components/all-accounts/all-accounts.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddTransactionComponent } from './components/add-transaction/add-transaction.component';


@NgModule({
  declarations: [
    AllAccountsComponent,
    AddTransactionComponent
  ],
  imports: [
    CommonModule,
    NgbModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    AccountRoutingModule
  ]
})
export class AccountModule { }
