import { Injectable } from '@angular/core';
import { FormGroup, ValidationErrors, FormControl } from '@angular/forms';

@Injectable({
    providedIn: 'root',
  })
export class FormValidatorHelper{
    constructor() {
    }

    GenerateErrorMessages(form : FormGroup) : { [key: string] : string }{
        const messages : { [key: string] : string } = {};
        Object.keys(form.controls).forEach(key => {
            const c = form.controls[key];
            if(c instanceof FormGroup){
                const childMessages = this.GenerateErrorMessages(c);
                Object.assign(messages,childMessages);
            }
            else if(form.controls[key].errors)
            {
                let errors = form.controls[key].errors;
                if(this.ErrorMessagesPerKeys[key]){
                    let e = (errors as ValidationErrors)
                    let errorKey = Object.keys(e)[0];
                    if(this.ErrorMessagesPerKeys[key][errorKey])
                    {
                        messages[key] = this.ErrorMessagesPerKeys[key][errorKey];
                    }
                }
            }
          });
          return messages;
    }

    requiredText = "Field Required!"
    ErrorMessagesPerKeys : { [key: string] : any } = {
        firstName : {
            required : this.requiredText,
            minlength : 'First Name too short!',
            maxlength : 'First Name too long!',
            pattern : 'Invalid name!'
        },
        lastName : {
            required : this.requiredText,
            minlength : 'Last Name too short!',
            maxlength : 'Last Name too long!',
            pattern : 'Invalid name!'
        },
        email : {
            required : this.requiredText,
            email : "Invalid email!",
            maxlength : 'Email too long!'
        },
        phoneNumber : {
            required : this.requiredText,
            pattern : 'Invalid phone number'

        },
        address :{
            maxlength : 'Address too long!'
        },
        balance :{
            required : this.requiredText,
            pattern : 'Invalid balance!'
        },
        currency:{
            required : this.requiredText,
            minlength : 'Invalid Currency!',
            maxlength : 'Invalid Currency!'
        },
        accountNumber:{
            require : this.requiredText,
            minlength : 'Invalid Account Number!',
            maxlength : 'Invalid Account Number!'
        },
        amount:{
            require : this.requiredText,
            min : 'You should put positive number!',
            max : 'You put more than account`s balance!'
        }
    }

    Regex = {
        phoneNumber : '^[+]?[0-9\\s\\-\\/]*$',
        name : '^[a-zA-Z\s]*$',
        password : '^((?=.*[a-z])(?=.*[A-Z])|(?=.*[\u0621-\u064A]))(?=.*\p{N})(?=.*[@\#%&_!=\{\}:‘\,/`~“;\$\^\*\+\.\?\(\)\[\]\|\-\\]).{8,}$',
        amount : /^-?\d+(?:\.\d+)?$/
    }

}
