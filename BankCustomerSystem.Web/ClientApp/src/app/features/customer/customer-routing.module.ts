import { AllCutomersComponent } from './components/all-cutomers/all-cutomers.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path:'all',
    component:AllCutomersComponent,
    data : {
    title : 'All Customers'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomerRoutingModule { }
