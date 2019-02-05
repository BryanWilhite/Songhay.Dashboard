import { NgModule } from '@angular/core';

import { AppDataService } from './services/songhay-app-data.service';
import { DomSanitizerUtility } from './services/songhay-dom-sanitzer.utility';

@NgModule({
    providers: [AppDataService, DomSanitizerUtility],
    exports: [AppDataService, DomSanitizerUtility]
})
export class SonghayAngularCoreModule {}
