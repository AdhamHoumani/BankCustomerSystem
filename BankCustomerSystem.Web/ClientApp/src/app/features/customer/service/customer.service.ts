import { Customer } from 'src/app/shared/models/sharedModel';
import { environment } from './../../../../environments/environment';
import { HttpHelper } from './../../../shared/helpers/HttpHelper';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  constructor(private httpHelper : HttpHelper) { }

  getAllCustomers(){
    return this.httpHelper.post(`${environment.baseCoreUrl}/Customer/GetAll`,{});
  }

  getCustomerById(customerId : string){
    return this.httpHelper.post(`${environment.baseCoreUrl}/Customer/GetById`,{customerId : customerId});
  }

  deleteCustomer(request : any){
    return this.httpHelper.post(`${environment.baseCoreUrl}/customer/delete`,request)
  }

  addCustomer(customer : Customer){
    return this.httpHelper.post(`${environment.baseCoreUrl}/customer`,customer)
  }

  updateCustomer(customer : Customer){
    return this.httpHelper.put(`${environment.baseCoreUrl}/customer`,customer)
  }
}
