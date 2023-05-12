
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { Language } from 'src/app/shared/enums/enums';
import { LocalStorageKeys } from '../../config/LocalStorageKeys';

@Component({
  selector: 'app-identity-template',
  templateUrl: './identity-template.component.html',
  styleUrls: ['./identity-template.component.css']
})
export class IdentityTemplateComponent implements OnInit {

  direction : any = 'ltr';
  constructor(public translate : TranslateService) { }
  ngOnInit(): void {
    if(localStorage.getItem(LocalStorageKeys.CurrentLang))
    {
      let lang = localStorage.getItem(LocalStorageKeys.CurrentLang) == 'ar' ? 'ar' : 'en';
      this.translate.use(lang);
      this.direction = localStorage.getItem(LocalStorageKeys.CurrentLang) == 'ar' ? 'rtl' : 'ltr'
    }
  }

  changeLanguage(lang : string){
    this.translate.use(lang);
    localStorage.setItem(LocalStorageKeys.CurrentLang,lang)
    this.changeDirection(lang)
  }

  changeDirection(lang : string)
  {
    switch(lang)
    {
      case 'en' :
        this.direction = 'ltr';
        break;
      case 'ar' :
        this.direction = 'rtl';
        break;
      default :
        this.direction = 'ltr';
        break;
    }
  }
}
