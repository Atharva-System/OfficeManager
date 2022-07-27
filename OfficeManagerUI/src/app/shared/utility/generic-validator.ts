import { FormGroup } from "@angular/forms";
import { VALIDATION_MESSAGE } from "./validation-messages";



export class GenericValidator {
    constructor(private validationMessages:{[key:string]:{[key:string]:string}} = VALIDATION_MESSAGE){}

    processMessages(container:FormGroup):{[key:string]:string} {
        const messages:{[key:string]:string} = {};
        for(const controlKey in container.controls)
        {
            if(container.controls.hasOwnProperty(controlKey)) {
                const controlProperty = container.controls[controlKey];
                if(controlProperty instanceof FormGroup)
                {
                    const childMessages = this.processMessages(controlProperty);
                    Object.assign(messages,childMessages);
                } else {
                    if(this.validationMessages[controlKey]){
                        messages[controlKey] = '';
                        if((controlProperty.dirty || controlProperty.touched) && controlProperty.errors)
                        {
                            Object.keys(controlProperty.errors).map(messageKey => {
                                if (this.validationMessages[controlKey][messageKey]) {
                                  messages[controlKey] += this.validationMessages[controlKey][messageKey] + ' ';
                                }
                            });
                        }
                    }
                }
            }
        }
        return messages;
    }
}