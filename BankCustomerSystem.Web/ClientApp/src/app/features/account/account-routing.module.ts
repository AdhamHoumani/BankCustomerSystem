import { AddTransactionComponent } from './components/add-transaction/add-transaction.component';
import { AllAccountsComponent } from './components/all-accounts/all-accounts.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path : 'all',
    component : AllAccountsComponent,
    data : {
      title : 'All Accounts'
    }
  },
  {
    path : 'transaction',
    component : AddTransactionComponent,
    data : {
      title : 'Create Transaction'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
