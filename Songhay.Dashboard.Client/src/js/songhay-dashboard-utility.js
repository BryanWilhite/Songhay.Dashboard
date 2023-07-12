import { DomUtility } from 'songhay';
export class SonghayDashboardUtility {
    static initializeBulma() {
        const burger = document.querySelector('.navbar-burger');
        if (!burger) {
            console.error('SonghayDashboardUtility:', { burger });
            return;
        }
        const target = burger['dataset']['target'];
        const nav = document.querySelector(`#${target}`);
        const isActiveCssClass = 'is-active';
        burger === null || burger === void 0 ? void 0 : burger.addEventListener('click', () => {
            burger.classList.toggle(isActiveCssClass);
            nav === null || nav === void 0 ? void 0 : nav.classList.toggle(isActiveCssClass);
        });
    }
}
DomUtility.runWhenWindowContentLoaded(() => SonghayDashboardUtility.initializeBulma());
//# sourceMappingURL=songhay-dashboard-utility.js.map