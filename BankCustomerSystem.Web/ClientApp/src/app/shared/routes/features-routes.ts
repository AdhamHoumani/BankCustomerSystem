import { Routes } from '@angular/router';
export const FEATURES_ROUTES: Routes = [
    {
        path:'customer',
        loadChildren:()=>import('../../features/customer/customer.module').then(m=>m.CustomerModule)
    },
    {
        path:'account',
        loadChildren:()=>import('../../features/account/account.module').then(m=>m.AccountModule)
    }
];

