/**
 * static members for @type {Object}
 *
 * @export
 * @class ObjectUtility
 */
export class ObjectUtility {
    /**
     * mutates object properties to lowercase
     *
     * @static
     * @param {{}} o
     * @param {number} [charIndex=0]
     * @returns {{}}
     * @memberof ObjectUtility
     */
    static lowerCasePropertyChar(o: {}, charIndex: number = 0): {} {
        if (!o) {
            return o;
        }
        const lowerCase = (s: string) => {
            return `${s.charAt(charIndex)}${s.slice(charIndex + 1)}`;
        };

        Object.keys(o).forEach(lowerCase);

        return o;
    }
}
