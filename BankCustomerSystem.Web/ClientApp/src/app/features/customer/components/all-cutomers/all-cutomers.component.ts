import { SharedService } from './../../../../shared/services/SharedService';
import { CustomerService } from '../../service/customer.service';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormGroup, FormBuilder, Validators, ValidationErrors, ValidatorFn, Form } from '@angular/forms';
import { FormValidatorHelper } from 'src/app/shared/helpers/FormValidatorHelper';
import { CommunicationService } from 'src/app/shared/services/Communication.service';
import { Router } from '@angular/router';
import { CommunicationKeys } from 'src/app/shared/config/CommunicationKeys';
import { Customer } from 'src/app/shared/models/sharedModel';

@Component({
  selector: 'app-all-cutomers',
  templateUrl: './all-cutomers.component.html',
  styleUrls: ['./all-cutomers.component.css']
})
export class AllCutomersComponent implements OnInit {

  customerForm : FormGroup = new FormGroup('');
  displayMessages : { [key : string] : string } = {};
  customers : Customer[] = [];
  openAddCustomerComponentB : boolean = false;
  saveClicked = false;
  isUpdateAction = false;
  customerIdToUpdate = '';
  customerIdToDelete = '';

  constructor(public translate : TranslateService,
    private sharedService : SharedService,
    private customerService : CustomerService,
    private validatorHelper : FormValidatorHelper,
    private communicationService : CommunicationService,
    private router : Router,
    private ngModal : NgbModal,
    private mdoalConfig : NgbModalConfig)
    {
      this.mdoalConfig.size = 'lg';
    }

  ngOnInit(): void {
    this.getAllCustomers();
    this.initCustomerForm();
  }

  // service methods
  getAllCustomers(){
    this.customerService.getAllCustomers().subscribe((res : any)=>
    {
      this.customers = res
    });
  }
  deleteCustomer(customerId : string, withCheckingAccounts : boolean, modal : any){
    this.customerIdToDelete = customerId;
    let request =
    {
      customerId : customerId,
      withCheckingAccounts : withCheckingAccounts
    }
    this.customerService.deleteCustomer(request).subscribe(res=>
      {
        if(res != null){
          console.log(res)
          if(res == true)
            this.customers = this.customers.filter(c=>c.id != customerId)
          else
            this.ngModal.open(modal)
        }
    })
    this.close();
  }

  updateCustomer(customer : Customer, modal : any){
    this.isUpdateAction = true;
    this.customerIdToUpdate = customer.id;
    this.initCustomerFormForUpdate(customer);
    this.ngModal.open(modal);
  }

  // Modal methods
  close(){
    this.ngModal.dismissAll();
  }
  getDirection(){
    return this.sharedService.getDirection();
  }
  openAddCustomerModal(modal : any){
    this.isUpdateAction = false;
    this.initCustomerForm();
    this.ngModal.open(modal);
  }

  // Form Methods
  initCustomerForm(){
    this.displayMessages = {};
    this.saveClicked = false;
    this.customerForm = new FormGroup(
      {
        firstName : new FormControl("",[Validators.required,Validators.minLength(2),Validators.maxLength(20),Validators.pattern(this.validatorHelper.Regex.name)]),
        lastName : new FormControl("",[Validators.required,Validators.minLength(2),Validators.maxLength(20),Validators.pattern(this.validatorHelper.Regex.name)]),
        email : new FormControl("",[Validators.required,Validators.email,Validators.maxLength(50)]),
        phoneNumber : new FormControl("",[Validators.required,Validators.pattern(this.validatorHelper.Regex.phoneNumber)]),
        address : new FormControl("",[Validators.maxLength(100)])
      });
      this.customerForm.valueChanges.subscribe(changes=>{
        if(this.saveClicked)
        {
          this.generateErrorMessages()
        }
      })
  }

  initCustomerFormForUpdate(customer : Customer){
    this.displayMessages = {};
    this.saveClicked = false;
    this.customerForm = new FormGroup(
      {
        firstName : new FormControl(customer.firstName,[Validators.required,Validators.minLength(2),Validators.maxLength(20),Validators.pattern(this.validatorHelper.Regex.name)]),
        lastName : new FormControl(customer.lastName,[Validators.required,Validators.minLength(2),Validators.maxLength(20),Validators.pattern(this.validatorHelper.Regex.name)]),
        email : new FormControl(customer.email,[Validators.required,Validators.email,Validators.maxLength(50)]),
        phoneNumber : new FormControl(customer.phoneNumber,[Validators.required,Validators.pattern(this.validatorHelper.Regex.phoneNumber)]),
        address : new FormControl(customer.address,[Validators.maxLength(100)])
      });
      this.customerForm.valueChanges.subscribe(changes=>{
        if(this.saveClicked)
        {
          this.generateErrorMessages()
        }
      })
  }

  generateErrorMessages(){
    this.displayMessages = this.validatorHelper.GenerateErrorMessages(this.customerForm);
  }
  submit(){
    this.saveClicked = true;
    this.generateErrorMessages();
    if(this.customerForm.valid){
      let customer : Customer = {
        id : this.isUpdateAction ? this.customerIdToUpdate : '00000000-0000-0000-0000-000000000000',
        firstName : this.customerForm.controls['firstName'].value,
        lastName : this.customerForm.controls['lastName'].value,
        email : this.customerForm.controls['email'].value,
        phoneNumber : this.customerForm.controls['phoneNumber'].value,
        address : this.customerForm.controls['address'].value,
      }
      if(this.isUpdateAction)
      {
        this.customerService.updateCustomer(customer).subscribe((res : any)=>{
          if(res != null){
            this.customers = this.customers.filter(c=>c.id != this.customerIdToUpdate)
            this.customers.push(res);
            this.isUpdateAction = false;
            this.ngModal.dismissAll()
          }
        })
      }
      else
      {
        this.customerService.addCustomer(customer).subscribe((res : any)=>{
          if(res != null){
            this.customers.push(res);
            this.isUpdateAction = false;
            this.ngModal.dismissAll()
          }
        })
      }
    }
  }

  //routing
  goToCustomerPage(id : string){
    this.communicationService.add(CommunicationKeys.CustomerId,id);
    this.router.navigate(['account/all']);
  }
}
