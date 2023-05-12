import { LocalStorageKeys } from './../config/LocalStorageKeys';
import { Injectable } from "@angular/core";

@Injectable({
    providedIn : 'root'
})
export class SharedService{
    getDirection(){
        let lang = localStorage.getItem(LocalStorageKeys.CurrentLang) ?? "en";
        return lang == "ar" ? "rtl" : "ltr";
    }

    isNullOrEmptyString(value : string){
        return value == '' || value == undefined || value == null || value == 'undefined'
    }
}
