import { ElementRef } from '@angular/core';

/**
 * static members for DOM manipulation
 *
 * @export
 * @class DomUtility
 */
export class DomUtility {
    /**
     * gets the element, extending @type {HTMLElement}
     * from the specified @type {ElementRef},
     * usually derived from @ViewChild
     *
     * @static
     * @template TElement
     * @param {ElementRef} elementRef
     * @returns {TElement}
     * @memberof DomUtility
     */
    static getHtmlElement<TElement extends HTMLElement>(
        elementRef: ElementRef
    ): TElement {
        const el = elementRef.nativeElement as TElement;

        if (!el) {
            console.warn('the expected element is not here');

            return;
        }

        return el;
    }

    /**
     * get an array of @type {Element}
     * from the specified collection
     *
     * @static
     * @param {HTMLCollection} collection
     * @returns {Element[]}
     * @memberof DomUtility
     */
    static getHtmlElements(collection: HTMLCollection): Element[] {
        const children = Array.from(collection);

        if (!children) {
            console.warn('the expected element children are not here');

            return;
        }

        if (!children.length) {
            console.warn('the expected number of element children is not here');

            return;
        }

        return children;
    }

    /**
     * gets the @type {CSSStyleDeclaration}
     * from the specified @type {Element}
     *
     * @static
     * @param {Element} element
     * @returns {CSSStyleDeclaration}
     * @memberof DomUtility
     */
    static getStyleDeclaration(element: Element): CSSStyleDeclaration {
        if (!element) {
            return;
        }

        const style = element['style'] as CSSStyleDeclaration;

        if (!style) {
            console.warn('the expected CSS style declaration is not here');
        }

        return style;
    }
}
