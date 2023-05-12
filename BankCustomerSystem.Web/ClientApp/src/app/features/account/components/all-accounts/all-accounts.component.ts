import { AccountType, AccountStatus, Transaction, AccountTypes, AccountStatuses } from './../../models/model';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { CommunicationKeys } from 'src/app/shared/config/CommunicationKeys';
import { CommunicationService } from 'src/app/shared/services/Communication.service';
import { SharedService } from 'src/app/shared/services/SharedService';
import { AccountService } from '../../service/account.service';
import { Account } from '../../models/model';
import { NgbModal, NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormValidatorHelper } from 'src/app/shared/helpers/FormValidatorHelper';

@Component({
  selector: 'app-all-accounts',
  templateUrl: './all-accounts.component.html',
  styleUrls: ['./all-accounts.component.css']
})
export class AllAccountsComponent implements OnInit {
  accounts : Account[] = []
  accountTransactions : Transaction[] = [];
  accountForm : FormGroup = new FormGroup('');
  displayMessages : { [key : string] : string } = {};
  accountTypes : AccountType[] = new AccountTypes().getTypes();
  accountStatuses : AccountStatus[] = new AccountStatuses().getStatuses();
  selectedAccountType = "INTERNAL"
  selectedAccountStatus = "ACTIVE"
  negativeBalanceClass = 'neg-balance';
  positiveBalanceClass = 'pos-balance';
  customerId = '';
  isUpdateAction = false;
  accountIdToUpdate = '';
  transactionsAccount : any
  saveClicked = false;
  constructor(public translate : TranslateService,
    private sharedService : SharedService,
    private validatorHelper : FormValidatorHelper,
    private accountService : AccountService,
    private communicationService : CommunicationService,
    private router : Router,
    private ngModal : NgbModal, private modalConfig : NgbModalConfig)
    {
      this.modalConfig.size = 'lg'
    }

  ngOnInit(): void {
    this.customerId = this.communicationService.getValue(CommunicationKeys.CustomerId);
    if(this.sharedService.isNullOrEmptyString(this.customerId)){
      this.router.navigate(['customer/all'])
      return;
    }
    this.getCustomerAccounts();
    this.initAccountForm();
  }

  // service functions
  getCustomerAccounts(){
    this.accountService.getAllAccountsByCustomerId(this.customerId).subscribe((res : any)=>{
      if(res){
        this.accounts = res;
      }
    })
  }
  updateCustomer(account : Account, modal : any){
    this.isUpdateAction = true;
    this.accountIdToUpdate = account.id;
    this.initAccountFormForUpdate(account);
    this.ngModal.open(modal);
  }
  deleteAccount(accountId : string){
    this.accountService.deleteAccount(accountId).subscribe(res=>
    {
      if(res != null){
        this.accounts = this.accounts.filter(c=>c.id != accountId)
      }
    })
  }

  // Form Methods
  initAccountForm(){
    this.displayMessages = {};
    this.saveClicked = false;
    this.accountForm = new FormGroup(
      {
        balance : new FormControl(0,[Validators.required,Validators.pattern(this.validatorHelper.Regex.amount)]),
        currency : new FormControl("",[Validators.required,Validators.minLength(2),Validators.maxLength(3)]),
        accountNumber : new FormControl("",[Validators.required,Validators.minLength(6),Validators.maxLength(15)]),
        accountStatus : new FormControl(),
        accountType : new FormControl(),
      });
      this.accountForm.valueChanges.subscribe(changes=>{
        if(this.saveClicked)
        {
          this.generateErrorMessages()
        }
      })
  }

  initAccountFormForUpdate(account : Account){
    this.displayMessages = {};
    this.saveClicked = false;
    this.selectedAccountStatus = account.accountStatus;
    this.selectedAccountType = account.accountType;
    this.accountForm = new FormGroup(
      {
        balance : new FormControl(account.balance,[Validators.required,Validators.pattern(this.validatorHelper.Regex.amount)]),
        currency : new FormControl(account.currency,[Validators.required,Validators.minLength(2),Validators.maxLength(3)]),
        accountNumber : new FormControl(account.accountNumber,[Validators.required,Validators.minLength(6),Validators.maxLength(15)]),
        accountStatus : new FormControl(),
        accountType : new FormControl(),
      });
      this.accountForm.valueChanges.subscribe(changes=>{
        if(this.saveClicked)
        {
          this.generateErrorMessages()
        }
      })
  }
  generateErrorMessages(){
    this.displayMessages = this.validatorHelper.GenerateErrorMessages(this.accountForm);
  }
  submit(){
    this.saveClicked = true;
    this.generateErrorMessages();
    if(this.accountForm.valid){
      let account : Account = {
        id : this.isUpdateAction ? this.accountIdToUpdate : '00000000-0000-0000-0000-000000000000',
        customerId : this.customerId,
        balance : this.accountForm.controls['balance'].value,
        initialBalance : this.accountForm.controls['balance'].value,
        currency : this.accountForm.controls['currency'].value,
        accountNumber : this.accountForm.controls['accountNumber'].value,
        accountStatus : this.selectedAccountStatus,
        accountType : this.selectedAccountType,
      }
      if(this.isUpdateAction){
          this.accountService.updateAccountToCustomer(account).subscribe((res : any)=>{
            this.accounts = this.accounts.filter(c=>c.id != this.accountIdToUpdate)
            this.accounts.push(res);
            this.isUpdateAction = false;
            this.ngModal.dismissAll()
          })
      }
      else{
        this.accountService.addAccountToCustomer(account).subscribe((res : any)=>{
          if(res){
            this.ngModal.dismissAll()
            this.isUpdateAction = false;
            this.accounts.push(res);
          }
        });
      }
    }
  }

  // Modal methods
  close(){
    this.ngModal.dismissAll();
  }
  getDirection(){
    return this.sharedService.getDirection();
  }
  openAddAccountModal(modal : any){
    this.isUpdateAction = false;
    this.initAccountForm();
    this.ngModal.open(modal);
  }
  openTransactionsTemplate(account : Account, modal : any){
    this.transactionsAccount = account;
    this.accountService.getAccountTransactions(account.id).subscribe((res : any)=>{
      if(res){
        this.accountTransactions = res;
        this.ngModal.open(modal);
      }
    })
  }

  //other methods
  addTransaction(transactionType : string){
    this.communicationService.add(CommunicationKeys.TransactionType,transactionType);
    this.communicationService.add(CommunicationKeys.AccountBalance,this.transactionsAccount.balance);
    this.communicationService.add(CommunicationKeys.AccountId,this.transactionsAccount.id);
    this.ngModal.dismissAll();
    this.router.navigate(['account/transaction']);
  }
  back(){
    this.router.navigate(['customer/all'])
  }
}
