import { Output, EventEmitter } from '@angular/core';

export class DataServiceMock {
    @Output()
    appDataLoaded: EventEmitter<any>;

    @Output()
    productsLoaded: EventEmitter<any>;

    @Output()
    twitterItemsLoaded: EventEmitter<{}>;

    constructor() {
        this.appDataLoaded = new EventEmitter();
        this.productsLoaded = new EventEmitter();
        this.twitterItemsLoaded = new EventEmitter();
    }
}
