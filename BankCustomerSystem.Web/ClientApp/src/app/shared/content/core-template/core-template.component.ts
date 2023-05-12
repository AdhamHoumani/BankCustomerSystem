import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit } from '@angular/core';
import { LocalStorageKeys } from 'src/app/shared/config/LocalStorageKeys';

@Component({
  selector: 'app-core-template',
  templateUrl: './core-template.component.html',
  styleUrls: ['./core-template.component.css']
})
export class CoreTemplateComponent implements OnInit {

  constructor(public translate : TranslateService,
    private router : Router) { }

  direction : any = 'ltr';
  // classes
  navbarltr = 'navbar-ltr dark-blue';
  navbarrtl = 'navbar-rtl dark-blue';
  navbarClass = 'navbar-ltr dark-blue';

  contentltr = 'content-ltr';
  contentrtl = 'content-rtl';
  contentClass = 'content-ltr';

  footerltr = 'footer-ltr light-blue';
  footerrtl = 'footer-rtl light-blue';
  footerClass = 'footer-ltr light-blue';

  ngOnInit(): void {
    if(localStorage.getItem(LocalStorageKeys.CurrentLang))
    {
      let lang = localStorage.getItem(LocalStorageKeys.CurrentLang) == 'ar' ? 'ar' : 'en';
      this.translate.use(lang);
      this.direction = localStorage.getItem(LocalStorageKeys.CurrentLang) == 'ar' ? 'rtl' : 'ltr'
      if(this.direction == 'rtl'){
        this.setClasses('rtl');
      }
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
    this.setClasses(this.direction)
  }

  setClasses(dir : string){
    switch (dir){
      case 'ltr' :
        this.navbarClass = this.navbarltr;
        this.contentClass = this.contentltr;
        this.footerClass = this.footerltr;
        break;
      case 'rtl' :
        this.navbarClass = this.navbarrtl;
        this.contentClass = this.contentrtl;
        this.footerClass = this.footerrtl;
        break;
      default :
        this.navbarClass = this.navbarltr;
        this.contentClass = this.contentltr;
        this.footerClass = this.footerltr;
        break;
    }
  }

  logout(){
    this.router.navigateByUrl('login');
  }
}
