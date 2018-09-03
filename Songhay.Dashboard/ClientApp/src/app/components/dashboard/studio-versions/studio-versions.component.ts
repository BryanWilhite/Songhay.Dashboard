import { Component, OnInit, VERSION } from '@angular/core';

import { DashboardDataService } from '../../../services/dashboard-data.service';

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

    /**
     * server info
     *
     * @type {string}
     * @memberof AppComponent
     */
    serverAssemblyInfo: string;

    /**
     * server version
     *
     * @type {string}
     * @memberof AppComponent
     */
    serverAssemblyVersion: string;

    constructor(public dashService: DashboardDataService) {
        this.clientFrameworkVersion = `${VERSION.major}.${VERSION.minor}.${
            VERSION.patch
        }`;
    }

    /**
     * implementing {OnInit}
     *
     * @memberof StudioFeedComponent
     */
    ngOnInit() {
        this.dashService.appDataLoaded.subscribe(() => {
            this.serverAssemblyInfo = `${
                this.dashService.assemblyInfo.assemblyTitle
            } ${this.dashService.assemblyInfo.assemblyVersion} ${
                this.dashService.assemblyInfo.assemblyCopyright
            }`;

            this.serverAssemblyVersion = this.dashService.assemblyInfo.assemblyVersion;
        });
    }
}
