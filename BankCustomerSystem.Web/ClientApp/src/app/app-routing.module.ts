import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { CoreTemplateComponent } from './shared/content/core-template/core-template.component';
import { IdentityTemplateComponent } from './shared/content/identity-template/identity-template.component';
import { FEATURES_ROUTES } from './shared/routes/features-routes';
import { IDENTITY_ROUTES } from './shared/routes/identity-routes';

const routes: Routes = [
  {
    path:'',
    redirectTo:'customer/all',
    pathMatch : 'full'
  },
  {
    path:'',component:CoreTemplateComponent,data:{title:'content views'},children:FEATURES_ROUTES
  },
  {
    path:'',component:IdentityTemplateComponent,data:{title:'auth views'},children:IDENTITY_ROUTES
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {preloadingStrategy:PreloadAllModules,relativeLinkResolution:'legacy'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
