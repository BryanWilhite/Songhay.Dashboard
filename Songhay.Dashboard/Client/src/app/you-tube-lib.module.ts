import { NgModule } from '@angular/core';
import {
    YouTubeModule,
    YouTubeRoutingModule
} from '@songhay/player-video-you-tube';

@NgModule({
    declarations: [],
    imports: [YouTubeModule, YouTubeRoutingModule],
    exports: [YouTubeModule, YouTubeRoutingModule]
})
export class YouTubeLibModule {}
