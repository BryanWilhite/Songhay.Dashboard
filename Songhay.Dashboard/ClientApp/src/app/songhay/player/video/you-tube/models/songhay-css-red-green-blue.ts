/**
 * defines CSS RGB
 *
 * @export
 * @class CssRedGreenBlue
 */
export class CssRedGreenBlue {
    /**
     * Creates an instance of CssRedGreenBlue.
     * @param {number} r
     * @param {number} g
     * @param {number} b
     * @memberof CssRedGreenBlue
     */
    constructor(r: number, g: number, b: number) {
        this.r = r;
        this.g = g;
        this.b = b;
    }

    /**
     * red
     *
     * @type {number}
     * @memberof CssRedGreenBlue
     */
    r: number;

    /**
     * green
     *
     * @type {number}
     * @memberof CssRedGreenBlue
     */
    g: number;

    /**
     * blue
     *
     * @type {number}
     * @memberof CssRedGreenBlue
     */
    b: number;
}
