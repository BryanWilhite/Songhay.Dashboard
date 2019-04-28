import { AfterViewInit, Component } from '@angular/core';

import { DomSanitizer } from '@angular/platform-browser';
import { MatIconRegistry } from '@angular/material';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements AfterViewInit {
    constructor(iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {
        iconRegistry.addSvgIconSetInNamespace(
            'rx',
            sanitizer.bypassSecurityTrustResourceUrl('assets/svg/sprites.svg')
        );
    }

    async ngAfterViewInit(): Promise<void> {
        const timeout = (ms: number) => {
            return new Promise(resolve => setTimeout(resolve, ms));
        };

        await timeout(3000);

        const main = Array.from(document.getElementsByTagName('main')).find(i => true) as HTMLMainElement;
        main.classList.add('hidden');

        await timeout(1000);

        main.classList.add('collapsed');
    }
}
