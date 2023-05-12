import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { IdentityTemplateComponent } from './shared/content/identity-template/identity-template.component';
import { CoreTemplateComponent } from './shared/content/core-template/core-template.component';
import { ToastrModule } from 'ngx-toastr';
import { SharedModule } from './shared/shared.module';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { InterceptorHelper } from './shared/helpers/InterceptorHelper.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    CoreTemplateComponent,
    IdentityTemplateComponent
  ],
  imports: [
    ToastrModule.forRoot({
      timeOut: 7000,
      positionClass: 'toast-top-center',
    }),
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule,
    NgxSpinnerModule,
    BrowserAnimationsModule,
    TranslateModule.forRoot({
      defaultLanguage : 'en',
      loader:{
        provide : TranslateLoader,
        useFactory : createTranslateLoader,
        deps : [HttpClient]
      }
    })
  ],
  providers: [
    {provide : HTTP_INTERCEPTORS, useClass : InterceptorHelper, multi : true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function createTranslateLoader(http:HttpClient){
  return new TranslateHttpLoader(http,'./assets/i18n/','.json')
}
