import { TranslateService } from '@ngx-translate/core';
import { LocalStorageKeys } from './../config/LocalStorageKeys';
import { Injectable } from "@angular/core";
import { ToastrService,IndividualConfig } from 'ngx-toastr';
import { ApiMessage, ApiMessageTypeEnum } from "../models/httpCallsModels";

@Injectable({
    providedIn: 'root',
  })

export class ToastrHelper{

    constructor(private toastrService : ToastrService, private translate : TranslateService) {

    }

    showMessages(messages : ApiMessage[])
    {
        let lang = localStorage.getItem(LocalStorageKeys.CurrentLang);
        const options: Partial<IndividualConfig> =
        {
            // positionClass : 'toast-top-center',
            // timeOut: 60000,
            messageClass : lang == 'ar' ? "toast-message-rtl" : "toast-message-ltr"
        };
        messages.forEach(
            m=>{
                switch(m.messageType)
                {
                    case ApiMessageTypeEnum.Success :
                        this.translate.get(m.message).subscribe(message =>
                        {
                            this.toastrService.success(message,'',options)
                        })
                        break;
                    case ApiMessageTypeEnum.Error :
                        this.translate.get(m.message).subscribe(message =>
                        {
                            this.toastrService.error(message,'',options)
                        })
                        break;
                    case ApiMessageTypeEnum.Warning :
                        this.translate.get(m.message).subscribe(message =>
                        {
                            this.toastrService.warning(message,'',options)
                        })
                        break;
                    case ApiMessageTypeEnum.Information :
                        this.translate.get(m.message).subscribe(message =>
                        {
                            this.toastrService.info(message,'',options)
                        })
                        break;
                }
            }
        )
    }
}
