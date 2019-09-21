import { NgModule } from '@angular/core';

import { YouTubeModule, YouTubeOptions } from '@songhay/player-video-you-tube';

import { YOU_TUBE_OPTIONS } from './models/you-tube-options';

@NgModule({
    declarations: [],
    imports: [YouTubeModule.forRoot(YOU_TUBE_OPTIONS)],
    exports: [YouTubeModule],
    providers: [{ provide: YouTubeOptions, useValue: YOU_TUBE_OPTIONS }]
})
export class YouTubeLibModule {}
