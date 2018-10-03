import { Output, EventEmitter } from '@angular/core';

export class DataServiceMock {
    @Output()
    appDataLoaded: EventEmitter<any>;

    @Output()
    productsLoaded: EventEmitter<any>;

    constructor() {
        this.appDataLoaded = new EventEmitter();
        this.productsLoaded = new EventEmitter();
    }
}
