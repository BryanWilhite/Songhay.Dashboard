import { DomUtility } from 'songhay';

export class SonghayDashboardUtility {
    static initializeBulma(): void {
        const burger = document.querySelector('.navbar-burger');

        if (!burger) {
            console.error('SonghayDashboardUtility:', {burger});
            return;
        }

        const target = burger['dataset']['target'];
        const nav = document.querySelector(`#${target}`);
        const isActiveCssClass = 'is-active';

        burger?.addEventListener('click', () => {
            burger.classList.toggle(isActiveCssClass);
            nav?.classList.toggle(isActiveCssClass);
        });
    }
}

DomUtility.runWhenWindowContentLoaded(() => SonghayDashboardUtility.initializeBulma());
