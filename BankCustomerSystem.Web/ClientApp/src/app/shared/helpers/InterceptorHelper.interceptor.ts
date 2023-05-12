import { HeadersKeys } from './../config/HeadersKeys';
import { catchError, finalize, tap } from 'rxjs/operators';
import { CallsStateService } from './../services/CallsState.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class InterceptorHelper implements HttpInterceptor {

  constructor(private callsStateService : CallsStateService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    console.log(request)
    let dontLockScreen = request.headers.has(HeadersKeys.DontLockScreen);
    if(!dontLockScreen){
      request.headers.delete(HeadersKeys.DontLockScreen)
      this.callsStateService.httpStarted();
    }
    return next.handle(request).pipe(
      finalize(()=>{
        if(!dontLockScreen)
          this.callsStateService.httpEnded();
      }),tap((result : any) =>{
      }), catchError(
        (err) => {
          console.log(err);
          throw err;
        }
      )
    );
  }
}
