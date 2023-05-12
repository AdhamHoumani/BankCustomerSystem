import { environment } from './../../../environments/environment';
import { HeadersKeys } from './../config/HeadersKeys';
import { ToastrHelper } from './ToastrHelper';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from "@angular/core";
import { catchError, map } from 'rxjs/operators';
import { ApiResponse, ApiStatusEnum } from '../models/httpCallsModels';
import { LocalStorageKeys } from '../config/LocalStorageKeys';

@Injectable({
    providedIn: 'root',
  })
export class HttpHelper{
    constructor(private http : HttpClient,
        private toastr : ToastrHelper) {
    }

    get<T>(url : string, lockScreen = true){
        let headers = new HttpHeaders();
        if(!lockScreen){
            headers = headers.append(HeadersKeys.DontLockScreen,"1");
        }
        headers = headers.append(HeadersKeys.Language,localStorage.getItem(LocalStorageKeys.CurrentLang) == 'ar' ? 'ar' : 'en')
        return this.http.get<ApiResponse>(url,{headers:headers}).pipe(map((response=>{
            this.toastr.showMessages(response.userMessages);
            if(response.status == ApiStatusEnum.Success)
            {
                return response.data as T;
            }
            if(environment.isDebuggingEnv)
                console.log(response.developerMessage)
            return null;
        })),catchError((err,ca)=> {
            if(environment.isDebuggingEnv)
                console.log(err)
            throw err;
        }))
    }

    post<T>(url : string, body : any, lockScreen = true){
        let headers = new HttpHeaders();
        if(!lockScreen){
            headers = headers.append(HeadersKeys.DontLockScreen,"1");
        }
        headers = headers.append(HeadersKeys.Language,localStorage.getItem(LocalStorageKeys.CurrentLang) == 'ar' ? 'ar' : 'en')
        return this.http.post<ApiResponse>(url,body,{headers:headers}).pipe(map((response=>{
            this.toastr.showMessages(response.userMessages);
            if(response.status == ApiStatusEnum.Success)
            {
                return response.data as T;
            }
            if(environment.isDebuggingEnv)
                console.log(response.developerMessage)
            return null;
        })),catchError((err,ca)=> {
            if(environment.isDebuggingEnv)
                console.log(err)
            throw err;
        }))
    }

    put<T>(url : string, body : any, lockScreen = true){
        let headers = new HttpHeaders();
        if(!lockScreen){
            headers = headers.append(HeadersKeys.DontLockScreen,"1");
        }
        headers = headers.append(HeadersKeys.Language,localStorage.getItem(LocalStorageKeys.CurrentLang) == 'ar' ? 'ar' : 'en')
        return this.http.put<ApiResponse>(url,body,{headers:headers}).pipe(map((response=>{
            this.toastr.showMessages(response.userMessages);
            if(response.status == ApiStatusEnum.Success)
            {
                return response.data as T;
            }
            if(environment.isDebuggingEnv)
                console.log(response.developerMessage)
            return null;
        })),catchError((err,ca)=> {
            if(environment.isDebuggingEnv)
                console.log(err)
            throw err;
        }))
    }

    delete<T>(url : string, lockScreen = true){
        let headers = new HttpHeaders();
        if(!lockScreen){
            headers = headers.append(HeadersKeys.DontLockScreen,"1");
        }
        headers = headers.append(HeadersKeys.Language,localStorage.getItem(LocalStorageKeys.CurrentLang) == 'ar' ? 'ar' : 'en')
        return this.http.delete<ApiResponse>(url,{headers:headers}).pipe(map((response=>{
            this.toastr.showMessages(response.userMessages);
            if(response.status == ApiStatusEnum.Success)
            {
                return response.data as T;
            }
            if(environment.isDebuggingEnv)
                console.log(response.developerMessage)
            return null;
        })),catchError((err,ca)=> {
            if(environment.isDebuggingEnv)
                console.log(err)
            throw err;
        }))
    }
}
