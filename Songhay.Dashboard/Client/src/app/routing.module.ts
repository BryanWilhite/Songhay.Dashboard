import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { StudioToolsComponent } from './components/dashboard/studio-tools/studio-tools.component';

import { AmazonProductImagesComponent } from './components/affiliates/amazon-product-images/amazon-product-images.component';
import { TweetedLinksBuilderComponent } from './components/social/tweeted-links-builder/tweeted-links-builder.component';
import { YouTubeRoutePaths } from '@songhay/player-video-you-tube';

const routes: Routes = [
    { path: '', redirectTo: 'dash', pathMatch: 'full' },
    { path: 'dash', component: DashboardComponent },
    { path: 'dash/tools', component: StudioToolsComponent },
    { path: 'affiliates/amazon/products/images', component: AmazonProductImagesComponent },
    { path: 'social/twitter/builder', component: TweetedLinksBuilderComponent },
    { path: `dash/${YouTubeRoutePaths.root}/${YouTubeRoutePaths.uploads}`,
        redirectTo: `${YouTubeRoutePaths.root}/${YouTubeRoutePaths.uploads}` },
    { path: `${YouTubeRoutePaths.root}/${YouTubeRoutePaths.uploads}`,
        loadChildren: () => import('./you-tube-lib.module').then(m => m.YouTubeLibModule) }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class RoutingModule { }
