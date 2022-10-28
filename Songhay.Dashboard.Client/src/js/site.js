(() => {

    window.addEventListener('DOMContentLoaded', () => {
        const burger = document.querySelector('.navbar-burger');
        const nav = document.querySelector(`#${burger.dataset.target}`);
        const isActiveCssClass = 'is-active';

        burger.addEventListener('click', () => {
            burger.classList.toggle(isActiveCssClass);
            nav.classList.toggle(isActiveCssClass);
        });
    });

})();
