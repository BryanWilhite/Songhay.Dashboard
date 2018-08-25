import { Component, OnInit, VERSION } from '@angular/core';

@Component({
    selector: 'app-studio-versions',
    templateUrl: './studio-versions.component.html',
    styleUrls: ['./studio-versions.component.scss']
})
export class StudioVersionsComponent implements OnInit {
    /**
     * App version
     *
     * @type {string}
     * @memberof AppComponent
     */
    clientFrameworkVersion: string;

    constructor() {
        this.clientFrameworkVersion = `${VERSION.major}.${VERSION.minor}.${
            VERSION.patch
        }`;
    }

    ngOnInit() {}
}
