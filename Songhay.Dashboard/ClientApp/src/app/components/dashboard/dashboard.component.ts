import { Component, OnInit } from '@angular/core';

import { DashboardDataService } from '../../services/dashboard-data.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    constructor(public dashService: DashboardDataService) {}

    ngOnInit(): void {
      this.dashService.loadAppData();
  }
}
