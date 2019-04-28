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

        if (!main) {
            console.warn('The expected main splash element is not here.');
            return;
        }

        const dialogClass = 'mdc-dialog--open';
        const dialog = Array.from(main.children).find(i => i.classList.contains(dialogClass));

        if (!dialog) {
            console.warn('The expected main dialog splash element is not here.');
            return;
        }

        dialog.classList.remove(dialogClass);
        dialog.classList.remove('mdc-dialog--closed');

        await timeout(100);

        main.classList.add('collapsed');
    }
}
