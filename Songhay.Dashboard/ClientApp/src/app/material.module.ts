import { NgModule } from '@angular/core';
import {
    MatButtonModule,
    MatCardModule,
    MatExpansionModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatToolbarModule
} from '@angular/material';

@NgModule({
    imports: [
        MatButtonModule,
        MatCardModule,
        MatExpansionModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatMenuModule,
        MatProgressSpinnerModule,
        MatSelectModule,
        MatToolbarModule
    ],
    exports: [
        MatButtonModule,
        MatCardModule,
        MatExpansionModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatMenuModule,
        MatProgressSpinnerModule,
        MatSelectModule,
        MatToolbarModule
    ]
})
export class MaterialModule {}
