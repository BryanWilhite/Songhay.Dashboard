import { ElementRef } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

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
     * gets the @type {HTMLHeadingElement}
     * from the specified heading level
     *
     * @static
     * @param {number} level
     * @returns {HTMLHeadingElement}
     * @memberof DomUtility
     * @see https://stackoverflow.com/a/51289849/22944
     */
    static getHtmlHeadingElement(level: number): HTMLHeadingElement {
        const heading =
            0 < level && level < 7
                ? (document.createElement(`h${level}`) as HTMLHeadingElement)
                : (document.createElement('h2') as HTMLHeadingElement);
        return heading;
    }

    /**
     * gets @type {SafeHtml} with the specified @type {DomSanitizer}
     *
     * @static
     * @param {DomSanitizer} sanitizer
     * @param {Element} element
     * @returns {SafeHtml}
     * @memberof DomUtility
     */
    static getSanitizedHtml(sanitizer: DomSanitizer, element: Element): SafeHtml {
        return sanitizer.bypassSecurityTrustHtml(element.outerHTML);
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
