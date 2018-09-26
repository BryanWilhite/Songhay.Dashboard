import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './components/dashboard/dashboard.component';
// TODO: see https://github.com/BryanWilhite/Songhay.Dashboard/issues/24
// import { YouTubeModule } from './songhay/player/video/you-tube/you-tube.module';

const routes: Routes = [
    { path: '', redirectTo: 'dash', pathMatch: 'full' },
    { path: 'dash', component: DashboardComponent },
    { path: 'player/video/youtube', loadChildren: './songhay/player/video/you-tube/you-tube.module' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class RoutingModule {}
