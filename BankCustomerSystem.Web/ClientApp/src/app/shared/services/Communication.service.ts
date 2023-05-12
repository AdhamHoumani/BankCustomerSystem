import { Injectable } from "@angular/core";

@Injectable({
    providedIn:"root"
})
export class CommunicationService {
    private elements : Map<string,any> = new Map<string,any>();

    public add(key:string,value:any){
        this.elements.set(key,value);
    }

    public getValue(key:string):any{
        return this.elements.get(key);
    }

    public clearByKey(key:string){
        this.elements.delete(key);
    }

    public clearAll(){
        this.elements.clear();
    }
}
