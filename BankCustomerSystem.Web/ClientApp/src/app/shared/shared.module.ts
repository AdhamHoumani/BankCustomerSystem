import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule,TranslateLoader } from '@ngx-translate/core';
import { SharedRoutingModule } from './shared-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';
import { NgxSpinnerModule } from 'ngx-spinner';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    SharedRoutingModule,
    HttpClientModule,
    TranslateModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    MatTooltipModule,
    FormsModule
  ],
  exports: [
    TranslateModule,
    HttpClientModule,
    TranslateModule,
    MatTooltipModule
  ]
})
export class SharedModule { }
