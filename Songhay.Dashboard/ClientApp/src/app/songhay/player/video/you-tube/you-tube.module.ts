import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './material.module';

import { YouTubeRoutingModule } from './you-tube-routing.module';

import { YouTubeDataService } from './services/you-tube-data.service';
import { YouTubePresentationDataServices } from './services/you-tube-presentation-data.services';

import { YouTubePresentationComponent } from './components/you-tube-presentation/you-tube-presentation.component';
import { YouTubeThumbsComponent } from './components/you-tube-thumbs/you-tube-thumbs.component';
import { YouTubeThumbsNavigationComponent } from './components/you-tube-thumbs-navigation/you-tube-thumbs-navigation.component';
import { YouTubeThumbsSetComponent } from './components/you-tube-thumbs-set/you-tube-thumbs-set.component';

@NgModule({
    imports: [CommonModule, MaterialModule, YouTubeRoutingModule],
    declarations: [
        YouTubePresentationComponent,
        YouTubeThumbsComponent,
        YouTubeThumbsNavigationComponent,
        YouTubeThumbsSetComponent
    ],
    providers: [
        YouTubeDataService,
        YouTubePresentationDataServices
    ],
    exports: [
        YouTubeRoutingModule,
        YouTubePresentationComponent,
        YouTubeThumbsComponent,
        YouTubeThumbsNavigationComponent,
        YouTubeThumbsSetComponent
    ]
})
export class YouTubeModule {}
