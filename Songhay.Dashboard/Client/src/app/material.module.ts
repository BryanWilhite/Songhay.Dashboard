import { NgModule } from '@angular/core';
import {
    MatButtonModule,
    MatCardModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatToolbarModule,
    MatTabsModule
} from '@angular/material';
import { DragDropModule } from '@angular/cdk/drag-drop';

@NgModule({
    imports: [
        MatButtonModule,
        MatCardModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatMenuModule,
        MatProgressSpinnerModule,
        MatSelectModule,
        MatTabsModule,
        MatToolbarModule,
        DragDropModule
    ],
    exports: [
        MatButtonModule,
        MatCardModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatMenuModule,
        MatProgressSpinnerModule,
        MatSelectModule,
        MatTabsModule,
        MatToolbarModule,
        DragDropModule
    ]
})
export class MaterialModule {}
