import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AmazonProductImagesComponent } from './components/affiliates/amazon-product-images/amazon-product-images.component';

const routes: Routes = [
    { path: '', redirectTo: 'dash', pathMatch: 'full' },
    { path: 'dash', component: DashboardComponent },
    { path: 'affiliates/amazon/products/images', component: AmazonProductImagesComponent },
    { path: 'player/video/youtube', loadChildren: './songhay/player/video/you-tube/you-tube.module#YouTubeModule' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class RoutingModule {}
