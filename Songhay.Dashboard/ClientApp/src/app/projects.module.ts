import { NgModule } from '@angular/core';
import { SonghayAngularCoreModule } from '../../projects/songhay-angular-core/src/public_api';
import { SonghayPlayerVideoModule } from '../../projects/songhay-player-video/src/public_api';

@NgModule({
    imports: [SonghayAngularCoreModule, SonghayPlayerVideoModule]
})
export class ProjectsModule {}
