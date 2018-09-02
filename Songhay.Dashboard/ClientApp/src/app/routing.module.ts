import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { YouTubeModule } from './songhay/player/video/you-tube/you-tube.module';

const routes: Routes = [
    { path: '', redirectTo: 'dash', pathMatch: 'full' },
    { path: 'dash', component: DashboardComponent },
    { path: 'player/video/youtube', loadChildren: () => YouTubeModule }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class RoutingModule {}
