import { Subscription } from 'rxjs';

import { Component, OnInit, VERSION, OnDestroy } from '@angular/core';

import { DashboardDataStore } from 'src/app/services/dashboard-data.store';

@Component({
    selector: 'app-studio-versions',
    templateUrl: './studio-versions.component.html',
    styleUrls: ['./studio-versions.component.scss']
})
export class StudioVersionsComponent implements OnInit, OnDestroy {
    /**
     * App version
     */
    clientFrameworkVersion: string;

    /**
     * server info
     */
    serverAssemblyInfo: string;

    /**
     * server version
     */
    serverAssemblyVersion: string;

    private subscriptions: Subscription[] = [];

    constructor(public dashStore: DashboardDataStore) {
        this.clientFrameworkVersion = `${VERSION.major}.${VERSION.minor}.${
            VERSION.patch
            }`;
    }

    /**
     * implementing {OnInit}
     */
    ngOnInit() {
        const sub = this.dashStore.serviceData.subscribe(() => {
            if (!this.dashStore.assemblyInfo) { return; }

            this.serverAssemblyInfo = `${
                this.dashStore.assemblyInfo.assemblyTitle
                } ${this.dashStore.assemblyInfo.assemblyVersion} ${
                this.dashStore.assemblyInfo.assemblyCopyright
                }`;

            this.serverAssemblyVersion = this.dashStore.assemblyInfo.assemblyVersion;
        });

        this.subscriptions.push(sub);
    }

    ngOnDestroy(): void {
        for (const sub of this.subscriptions) {
            sub.unsubscribe();
        }
    }

}
