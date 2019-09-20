import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';

// TODO: loading this module from the lib makes true lazy-loading impossible 😒
import { YouTubeModule } from '@songhay/player-video-you-tube';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './material.module';
import { RoutingModule } from './routing.module';

import { AmazonDataStore } from './services/amazon-data.store';
import { DashboardDataStore } from './services/dashboard-data.store';
import { SocialDataStore } from './services/social-data.store';

import { AppComponent } from './components/app.component';
import { AmazonProductImagesComponent } from './components/affiliates/amazon-product-images/amazon-product-images.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { StudioComponent } from './components/dashboard/studio/studio.component';
import { StudioFeedComponent } from './components/dashboard/studio-feed/studio-feed.component';
import { StudioLinksComponent } from './components/dashboard/studio-links/studio-links.component';
import { StudioLogoComponent } from './components/dashboard/studio-logo/studio-logo.component';
import { StudioNavComponent } from './components/dashboard/studio-nav/studio-nav.component';
import { StudioSocialComponent } from './components/dashboard/studio-social/studio-social.component';
import { StudioToolsComponent } from './components/dashboard/studio-tools/studio-tools.component';
import { StudioVersionsComponent } from './components/dashboard/studio-versions/studio-versions.component';
import { TweetedLinksBuilderComponent } from './components/social/tweeted-links-builder/tweeted-links-builder.component';

@NgModule({
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        BrowserAnimationsModule,
        FlexLayoutModule,
        HttpClientModule,
        MaterialModule,
        ReactiveFormsModule,
        RoutingModule,
        YouTubeModule
    ],
    declarations: [
        AppComponent,
        DashboardComponent,
        StudioComponent,
        StudioFeedComponent,
        StudioLinksComponent,
        StudioLogoComponent,
        StudioSocialComponent,
        StudioToolsComponent,
        StudioVersionsComponent,
        AmazonProductImagesComponent,
        TweetedLinksBuilderComponent,
        StudioNavComponent
    ],
    providers: [
        AmazonDataStore,
        DashboardDataStore,
        SocialDataStore
    ],
    bootstrap: [AppComponent]
})
export class AppModule {}
