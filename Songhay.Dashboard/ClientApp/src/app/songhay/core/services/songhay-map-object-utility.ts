/**
 * static members for @type {Map<K, V>}
 *
 * @export
 * @class MapObjectUtility
 */
export class MapObjectUtility {
    /**
     * gets @type {Map<string, TValue>} from an object
     *
     * @static
     * @template TValue
     * @param {{}} o
     * @param {(propertyName: string, propertyValue: any) => TValue} valueGetter
     * @returns {Map<string, TValue>}
     * @memberof MapObjectUtility
     */
    static getMap<TValue>(
        o: {},
        valueGetter: (propertyName: string, propertyValue: any) => TValue
    ): Map<string, TValue> {
        if (!o) {
            console.log('object to be mapped is not truthy');
            return null;
        }
        const iterable = Object.keys(o).map(propertyName => {
            const propertyValue = o[propertyName];
            const value = valueGetter(propertyName, propertyValue);
            return [propertyName, value] as [string, TValue];
        });
        return new Map<string, TValue>(iterable);
    }
}
