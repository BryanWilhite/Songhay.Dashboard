import { Component, OnInit } from '@angular/core';

import { Location } from '@angular/common';

@Component({
    selector: 'app-studio-nav',
    templateUrl: './studio-nav.component.html',
    styleUrls: ['./studio-nav.component.scss']
})
export class StudioNavComponent implements OnInit {
    constructor(private location: Location) {}

    ngOnInit() {}

    goBack() {
        this.location.back();
    }
}
