import { environment } from './../../../../environments/environment';
import { Account } from './../models/model';
import { HttpHelper } from '../../../shared/helpers/HttpHelper';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private httpHelper : HttpHelper) { }

  getAllAccountsByCustomerId(customerId : string){
    return this.httpHelper.get(`${environment.baseFinanceUrl}/account/${customerId}`);
  }

  addAccountToCustomer(account : Account){
    return this.httpHelper.post(`${environment.baseFinanceUrl}/account`,account);
  }

  updateAccountToCustomer(account : Account){
    return this.httpHelper.put(`${environment.baseFinanceUrl}/account`,account);
  }

  deleteAccount(accountId : string){
    return this.httpHelper.delete(`${environment.baseFinanceUrl}/account/${accountId}`);
  }

  getAccountTransactions(accountId : string){
    return this.httpHelper.get(`${environment.baseFinanceUrl}/transaction/${accountId}`);
  }

  createTransaction(transaction : any){
    return this.httpHelper.post(`${environment.baseFinanceUrl}/transaction`,transaction);
  }
}
