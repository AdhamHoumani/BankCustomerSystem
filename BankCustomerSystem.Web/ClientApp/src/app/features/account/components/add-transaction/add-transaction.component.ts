import { AccountService } from './../../service/account.service';
import { FormValidatorHelper } from 'src/app/shared/helpers/FormValidatorHelper';
import { Router } from '@angular/router';
import { CommunicationKeys } from 'src/app/shared/config/CommunicationKeys';
import { CommunicationService } from './../../../../shared/services/Communication.service';
import { TransactionStatuses, TransactionTypes } from './../../models/model';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-transaction',
  templateUrl: './add-transaction.component.html',
  styleUrls: ['./add-transaction.component.css']
})
export class AddTransactionComponent implements OnInit {

  transactionForm : FormGroup = new FormGroup({
    amount : new FormControl(),
    status : new FormControl()
  });
  transactionTypeLabel = '';
  transactionType = '';
  accountId = '';
  confirmClicked = false;
  accountBalance = 0;
  transactionStatuses = new TransactionStatuses().getStatuses();
  transactionStatus = this.transactionStatuses[0].name;
  transactionTypes = new TransactionTypes().getTypes();
  displayMessages : { [key : string] : string } = {};
  constructor(private communicationService : CommunicationService,
    private router : Router,
    private validatorHelper : FormValidatorHelper,
    private accountService : AccountService) { }

  ngOnInit(): void {
    this.transactionType = this.communicationService.getValue(CommunicationKeys.TransactionType)
    this.accountId = this.communicationService.getValue(CommunicationKeys.AccountId)
    this.accountBalance = this.communicationService.getValue(CommunicationKeys.AccountBalance);
    if(!this.transactionType || !this.accountId || !(this.accountBalance || this.accountBalance == 0)){
      this.back();
      return;
    }
    this.initTransactionForm();
    this.transactionTypeLabel = this.transactionTypes.filter(t=>t.name == this.transactionType)[0].description;
  }

  // from methods
  initTransactionForm(){
    this.transactionForm = new FormGroup({
      amount : new FormControl(0,this.getValidatorsByTransactionType()),
      status : new FormControl()
    })
    this.transactionForm.valueChanges.subscribe(changes=>{
      if(this.confirmClicked)
      {
        this.generateErrorMessages()
      }
    })
  }
  generateErrorMessages(){
    this.displayMessages = this.validatorHelper.GenerateErrorMessages(this.transactionForm);
  }
  getValidatorsByTransactionType(){
    return this.transactionType == 'DEPOSIT' ?
    [Validators.required,Validators.min(0)] :
    [Validators.required,Validators.min(0),Validators.max(this.accountBalance)]
  }


  submit(){
    this.confirmClicked = true;
    this.generateErrorMessages();
    if(this.transactionForm.valid){
      let transaction = {
        accountId : this.accountId,
        transactionStatus : this.transactionForm.controls['status'].value,
        transactionType : this.transactionType,
        amount : this.transactionForm.controls['amount'].value,
        createdOn : new Date()
      }
      this.accountService.createTransaction(transaction).subscribe(res=>{
        if(res){
          this.back()
        }
      })
    }
  }

  back(){
    this.communicationService.clearByKey(CommunicationKeys.AccountId);
    this.communicationService.clearByKey(CommunicationKeys.TransactionType);
    this.communicationService.clearByKey(CommunicationKeys.AccountBalance);
    this.router.navigate(['account/all']);
  }
}
