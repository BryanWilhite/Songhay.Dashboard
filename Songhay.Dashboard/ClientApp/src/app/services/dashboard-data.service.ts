import { Injectable } from '@angular/core';
import { AppDataService } from './songhay-app-data.service';
import { Http, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class DashboardDataService extends AppDataService {
    constructor(client: Http) {
        super(client);
    }
}
