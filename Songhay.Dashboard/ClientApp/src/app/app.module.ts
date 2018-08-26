import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './material.module';
import { RoutingModule } from './routing.module';

import { AppComponent } from './components/app.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { StudioComponent } from './components/dashboard/studio/studio.component';
import { StudioLogoComponent } from './components/dashboard/studio-logo/studio-logo.component';
import { StudioSocialComponent } from './components/dashboard/studio-social/studio-social.component';
import { StudioVersionsComponent } from './components/dashboard/studio-versions/studio-versions.component';
import { StudioLinksComponent } from './components/dashboard/studio-links/studio-links.component';

@NgModule({
    declarations: [
        AppComponent,
        DashboardComponent,
        StudioComponent,
        StudioLogoComponent,
        StudioSocialComponent,
        StudioVersionsComponent,
        StudioLinksComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        BrowserAnimationsModule,
        FlexLayoutModule,
        HttpClientModule,
        MaterialModule,
        ReactiveFormsModule,
        RoutingModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}
