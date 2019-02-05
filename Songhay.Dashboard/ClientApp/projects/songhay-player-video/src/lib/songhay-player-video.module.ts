import { NgModule } from '@angular/core';
import { YouTubeModule } from './you-tube/you-tube.module';

@NgModule({
  imports: [YouTubeModule],
  exports: [YouTubeModule]
})
export class SonghayPlayerVideoModule { }
