import { Output, EventEmitter } from '@angular/core';

export class DataServiceMock {
    @Output()
    appDataLoaded: EventEmitter<any>;

    constructor() {
        this.appDataLoaded = new EventEmitter();
    }
}
