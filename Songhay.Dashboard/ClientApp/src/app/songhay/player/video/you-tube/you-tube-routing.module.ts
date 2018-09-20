import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { YouTubeThumbsSetComponent } from './components/you-tube-thumbs-set/you-tube-thumbs-set.component';

const routes: Routes = [
    { path: '', redirectTo: 'uploads', pathMatch: 'full' },
    { path: 'uploads/:suffix/:id', component: YouTubeThumbsSetComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class YouTubeRoutingModule {}
