import { NgxSpinnerService } from 'ngx-spinner';
import { Injectable } from "@angular/core";

@Injectable({
    providedIn : 'root'
})

export class CallsStateService{
    isLoading = false;

    constructor(private ngxSpinner : NgxSpinnerService) {
    }

    httpStarted(){
        if(!this.isLoading){
            this.ngxSpinner.show(undefined,{
                type : 'line-spin-fade',
                size : 'medium',
                bdColor : 'rgba(0,0,0,0.2)',
                color : '#ffc200ee',
                fullScreen : true
            })
        }
        this.isLoading = true;
    }

    httpEnded(){
        this.isLoading = false;
        this.ngxSpinner.hide();
    }
}
