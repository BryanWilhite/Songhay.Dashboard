import { Injectable } from '@angular/core';
import { CssLinearGradientData } from '../models/songhay-css-linear-gradient-data';
import { CssRedGreenBlue } from '../models/songhay-css-red-green-blue';

/**
 * routines for inline CSS
 *
 * @export
 * @class CssUtility
 */
@Injectable()
export class CssUtility {
    /**
     * gets #ffffff format from 0xFFFFFF format
     *
     * @param {string} color
     * @returns {string}
     * @memberof CssUtility
     */
    static getColorHex(color: string): string {
        if (!color) {
            return null;
        }
        return color.replace('0x', '#').toLowerCase();
    }

    /**
     * gets r, g, b format from @type {CssRedGreenBlue} data
     *
     * @param {CssRedGreenBlue} rgb
     * @returns {string}
     * @memberof CssUtility
     */
    static getColorRgb(rgb: CssRedGreenBlue): string {
        if (!rgb) {
            return null;
        }
        return `${rgb.r}, ${rgb.g}, ${rgb.b}`;
    }

    /**
     * gets opacity 0.70 format from "70" format
     *
     * @param {string} opacity
     * @returns {number}
     * @memberof CssUtility
     */
    static getOpacity(opacity: string): number {
        return parseInt(opacity, 10) / 100;
    }

    /**
     * gets 70px pixel format from number 70
     *
     * @param {number} pixels
     * @returns {string}
     * @memberof CssUtility
     */
    static getPixelValue(pixels: number): string {
        return `${pixels}px`;
    }

    /**
     * gets @type {CssRedGreenBlue} data from #ffffff (or #fff) format
     *
     * @param {string} hex
     * @returns {CssRedGreenBlue}
     * @memberof CssUtility
     *
     * @see http://stackoverflow.com/questions/5623838/rgb-to-hex-and-hex-to-rgb/5624139?stw=2#5624139
     */
    static getRgbFromHex(hex: string): CssRedGreenBlue {
        // Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")
        const shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
        hex = hex.replace(shorthandRegex, function(r, g, b) {
            return r + r + g + g + b + b;
        });

        const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
        return result
            ? new CssRedGreenBlue(
                  parseInt(result[1], 16),
                  parseInt(result[2], 16),
                  parseInt(result[3], 16)
              )
            : null;
    }

    /**
     * get linear-gradient CSS from @type {CssLinearGradientData} data
     *
     * @param {CssLinearGradientData} data
     * @returns {string}
     * @memberof CssUtility
     */
    static getTintedBackground(data: CssLinearGradientData): string {
        if (!data) {
            return null;
        }
        const tintHex = CssUtility.getColorHex(data.backgroundColor);
        const tintColor = CssUtility.getColorRgb(CssUtility.getRgbFromHex(tintHex));
        const tintAlpha = data.alpha;
        const backgroundImage = CssUtility.getUri(data.backgroundImageUri);

        return `linear-gradient(rgba(${tintColor}, ${tintAlpha}), rgba(${tintColor}, ${tintAlpha})), ${backgroundImage}`;
    }

    /**
     * get url CSS from the URI
     *
     * @param {string} uri
     * @returns {string}
     * @memberof CssUtility
     */
    static getUri(uri: string): string {
        return `url('${uri}')`;
    }
}
