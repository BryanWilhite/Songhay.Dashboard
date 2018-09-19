import { Observable } from 'rxjs/Observable';
import { ReplaySubject } from 'rxjs/ReplaySubject';

import { Params } from '@angular/router';

export class ActivatedRouteMock {
    params: Observable<Params>;
    subject: ReplaySubject<Params>;
    constructor() {
        this.subject = new ReplaySubject<Params>();
        this.params = this.subject.asObservable();
    }
    nextParams(routeParams?: Params) {
        console.log('routeParams: ', routeParams);
        this.subject.next(routeParams);
    }
}
