(() => {

    window.addEventListener('DOMContentLoaded', () => {
        const burger = document.querySelector('.navbar-burger');
        const nav = document.querySelector(`#${burger.dataset.target}`);
    
        burger.addEventListener('click', () => {
            burger.classList.toggle('is-active');
            nav.classList.toggle('is-active');
        });
    });

})();
