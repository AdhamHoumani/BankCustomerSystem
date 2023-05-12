export interface Account{
    id : string;
    customerId : string;
    accountStatus : string;
    accountType : string;
    accountNumber : string;
    balance : number;
    initialBalance : number;
    currency : string;
}

export interface AccountStatus{
    name : string;
    description : string;
}

export interface AccountType{
    name : string;
    description : string;
}

export interface TransactionStatus{
    name : string;
    description : string;
}

export interface TransactionType{
    name : string;
    description : string;
}

export interface Transaction{
    id : string;
    accountId : string;
    transactionStatus : string;
    transactionType : string;
    amount : number;
    createdOn : Date;
}

export class AccountTypes {
    private data : AccountType[] = [{name : "EXTERNAL", description : "External"},{name : "INTERNAL", description : "Internal"}];
    getTypes(){
        return this.data;
    }
}
export class AccountStatuses {
    private data : AccountStatus[] = [{name : "ACTIVE", description : "Active"},{name : "ON_HOLD", description : "On Hold"}, {name : 'BLOCKED',description : 'Blocked'}];
    getStatuses(){
        return this.data;
    }
}
export class TransactionStatuses {
    private data : TransactionStatus[] = [{name : "SUCCESS", description : "Success"},{name : "FAILED", description : "Failed"}, {name : 'PENDING',description : 'Pending'}];
    getStatuses(){
        return this.data;
    }
}
export class TransactionTypes {
    private data : TransactionType[] = [{name : "DEPOSIT", description : "Deposit"},{name : "WITHDRAW", description : "Withdraw"}];
    getTypes(){
        return this.data;
    }
}
