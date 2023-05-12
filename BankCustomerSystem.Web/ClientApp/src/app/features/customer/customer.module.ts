import { SharedModule } from './../../shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerRoutingModule } from './customer-routing.module';
import { AllCutomersComponent } from './components/all-cutomers/all-cutomers.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    AllCutomersComponent
  ],
  imports: [
    CommonModule,
    NgbModule,
    SharedModule,
    ReactiveFormsModule,
    CustomerRoutingModule
  ]
})
export class CustomerModule { }
