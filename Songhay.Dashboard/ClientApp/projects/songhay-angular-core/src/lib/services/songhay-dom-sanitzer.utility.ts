import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

/**
 * static members for DOM manipulation
 *
 * @export
 * @class DomSanitizerUtility
 */
export class DomSanitizerUtility {
    /**
     * gets @type {SafeHtml} with the specified @type {DomSanitizer}
     *
     * @static
     * @param {DomSanitizer} sanitizer
     * @param {Element} element
     * @returns {SafeHtml}
     * @memberof DomSanitizerUtility
     */
    static getSanitizedHtml(sanitizer: DomSanitizer, element: Element): SafeHtml {
        return sanitizer.bypassSecurityTrustHtml(element.outerHTML);
    }
}
