import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AmazonProductImagesComponent } from './components/affiliates/amazon-product-images/amazon-product-images.component';
import { TweetedLinksBuilderComponent } from './components/social/tweeted-links-builder/tweeted-links-builder.component';
import { DragAndDropDemoComponent } from './components/demo/drag-drop-demo';

const routes: Routes = [
    { path: '', redirectTo: 'dash', pathMatch: 'full' },
    { path: 'dash', component: DashboardComponent },
    { path: 'affiliates/amazon/products/images', component: AmazonProductImagesComponent },
    { path: 'demo', component: DragAndDropDemoComponent },
    { path: 'social/twitter/builder', component: TweetedLinksBuilderComponent },
    { path: 'player/video/youtube', loadChildren: './songhay/player/video/you-tube/you-tube.module#YouTubeModule' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class RoutingModule {}
