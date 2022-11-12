var rx;(()=>{"use strict";var e={d:(t,r)=>{for(var n in r)e.o(r,n)&&!e.o(t,n)&&Object.defineProperty(t,n,{enumerable:!0,get:r[n]})},o:(e,t)=>Object.prototype.hasOwnProperty.call(e,t),r:e=>{"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})}},t={};e.r(t),e.d(t,{ArrayUtility:()=>i,CssLinearGradientData:()=>r,CssRedGreenBlue:()=>n,CssUtility:()=>a,DisplayItemUtility:()=>p,DomUtility:()=>d,FunctionDisplayModel:()=>s,MapObjectUtility:()=>l,MathUtility:()=>m,ObjectUtility:()=>g,ReducedGroupUtility:()=>c,SyndicationFeed:()=>o});class r{constructor(e,t,r){this.alpha=e,this.backgroundColor=t,this.backgroundImageUri=r}}class n{constructor(e,t,r){this.r=e,this.g=t,this.b=r}}class s{constructor(){this.description=null,this.memberName=null}}class o{constructor(){this.feedImage=null,this.feedItems=[],this.feedTitle=null}static getAtomChannelTitle(e){const t=e.feed;if(!t)return console.warn("the expected feed is not here"),null;const r=t.title,n="#text";return-1!==Object.keys(r).indexOf(n)?r[n]:r}static getAtomChannelItems(e){const t=e.feed.entry;return t?t.length?t:(console.warn("the expected feed items are not here"),[]):(console.warn("the expected feed root is not here"),[])}static getRssChannelItems(e){const t=e.rss.channel.item;return t?t.length?t:(console.warn("the expected RSS channel items are not here"),[]):(console.warn("the expected RSS channel root is not here"),[])}static getRssChannelTitle(e){const t=e.rss.channel;return t?t.title:(console.warn("the expected RSS channel is not here"),null)}}class i{static findDuplicates(e){if(!e)return[];const t=[];return e.forEach(((r,n)=>{-1!==e.indexOf(r,n+1)&&-1===t.indexOf(r)&&t.push(r)})),t}static groupBy(e,t){const r=e.reduce(((e,r,n=t(r))=>((e[n]||(e[n]=[])).push(r),e)),{}),n=[];for(const e in r)r.hasOwnProperty(e)&&n.push({key:e,values:r[e]});return n}static isNotTruthyOrIsEmpty(e){return!e||!e.length}static sortItems(e,t,r=!1,n=!1){if(!e)return e;const s=r?e.sort(((e,r)=>void 0===e.propertyName||void 0===r.propertyName?-1:+e[t]-+r[t])):e.sort(((e,r)=>{if(void 0===e.propertyName||void 0===r.propertyName)return-1;const n=e[t].toUpperCase(),s=r[t].toUpperCase();return n<s?-1:n>s?1:0}));return n?s.reverse():s}}class a{static getColorHex(e){return e?e.replace("0x","#").toLowerCase():null}static getColorRgb(e){return e?`${e.r}, ${e.g}, ${e.b}`:null}static getComputedStylePropertyValue(e,t){if(!e)return null;if(!t)return null;return window.getComputedStyle(e).getPropertyValue(t)}static getComputedStylePropertyValueById(e,t){if(!e)return null;if(!t)return null;const r=window.document.getElementById(e);if(!r)return null;return window.getComputedStyle(r).getPropertyValue(t)}static getComputedStylePropertyValueByQuery(e,t){if(!e)return null;if(!t)return null;const r=window.document.querySelector(e);if(!r)return null;return window.getComputedStyle(r).getPropertyValue(t)}static getOpacity(e){return parseInt(e,10)/100}static getPixelValue(e){return`${e}px`}static getRgbFromHex(e){if(!e)return null;e=e.replace(/^#?([a-f\d])([a-f\d])([a-f\d])$/i,((e,t,r)=>e+e+t+t+r+r));const t=/^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(e);return t?new n(parseInt(t[1],16),parseInt(t[2],16),parseInt(t[3],16)):null}static getTintedBackground(e){if(!e)return null;const t=this.getColorHex(e.backgroundColor),r=this.getColorRgb(this.getRgbFromHex(t)),n=e.alpha;return`linear-gradient(rgba(${r}, ${n}), rgba(${r}, ${n})), ${this.getUri(e.backgroundImageUri)}`}static getUri(e){return`url('${e}')`}static setComputedStylePropertyValue(e,t,r){e&&t&&r&&e.style.setProperty(t,r)}}class l{static getMap(e,t){if(!e)return console.warn("object to be mapped is not truthy"),null;const r=Object.keys(e).map((r=>{const n=e[r];return[r,t?t(r,n):n]}));return new Map(r)}static getMapFromKeyValuePairs(e,t){const r=e.reduce(((e,t)=>(e[t.key]=t.value,e)),{});return l.getMap(r,t)}static getObject(e){const t={};return e?(e.forEach(((e,r)=>t[r]=e)),t):t}}class c{static reduceToObject(e){const t=e.map((e=>{const t={};return t[e.key]=e.values,t})).reduce(((e,t)=>Object.assign(Object.assign({},t),e)),{});return console.log({reduction:t}),t}}const u={id:"zzz-group-none",displayText:"[no grouping]"};class p{static displayInGroups(e,t,r=!1){p.setGrouping(e,t);const n=p.group(e);return p.nestIntoGroups(n,r)}static getItemMapPair(e,t){const r=e=>{const t=["The expected selectable map group display text is not here.",` [ID: ${e||"[missing]"}]`].join("");console.warn(t)},n=()=>{console.warn("The expected item map is not here.",{item:e})};return t?(t=>{if(!e.map||!e.map.size)return n(),u;if(!e.map.has(t)&&!(t=Array.from(e.map.keys()).find((e=>e.toString().startsWith(t)))))return console.warn("The expected item map identifier is not here.",{item:e}),u;const s=e.map.get(t);return s||r(t),{id:t,displayText:s}})(t):(()=>{if(!e.map||!e.map.size)return n(),u;const t=Array.from(e.map.entries())[0],s=t[0],o=t[1];return o||r(s),{id:s,displayText:o}})()}static getStringifiableObject(e){const t=e.map?l.getObject(e.map):{};return Object.assign(Object.assign(Object.assign(Object.assign(Object.assign({},e),e),e),{map:t}),{childItems:e.childItems&&e.childItems.length?e.childItems.map((e=>p.getStringifiableObject(e))):[]})}static group(e){if(!e)throw new Error("The expected items are not here.");const t=i.groupBy(e,(e=>e.groupId));return c.reduceToObject(t)}static nestIntoGroups(e,t=!1){if(!e)throw new Error("The expected groups are not here.");return((e,t)=>{const r=e.sort();return t?r.reverse():r})(Object.keys(e),t).map((r=>{const n=e[r];if(!n.length)throw new Error("The expected grouping data format is not here.");const s=n[0];if(!s.groupId)throw new Error("The expected grouping identifier is not here.");if(!s.displayText)throw new Error("The expected grouping display text is not here.");return{id:s.groupId,displayText:s.groupDisplayText||"",childItems:i.sortItems(e[r],"sortOrdinal",!1,t)}}))}static setGrouping(e,t){if(!e)throw new Error("The expected items are not here.");e.filter((e=>!!e)).forEach((e=>{const r=p.getItemMapPair(e,t);r?(e.groupId=r.id,e.groupDisplayText=r.displayText):e.groupId&&e.displayText||(e=>{const t=["The expected Selectable map pair and/or groupId/displayText is not here.",` [Group ID: ${e.groupId||"[missing]"}]`,` [Group Display Text: ${e.displayText||"[missing]"}]`].join("");console.warn(t,{item:e})})(e)}))}static stringify(e){if(!e)return null;const t=p.getStringifiableObject(e);return JSON.stringify(t)}static stringifyAll(e){if(!e)return null;const t=e.map((e=>p.getStringifiableObject(e)));return JSON.stringify(t)}}class d{static getChildHtmlElementByQuery(e,t){return e?e.querySelector(t):(console.warn("the expected parent element is not here"),null)}static getChildHtmlElementsByQuery(e,t){return e?e.querySelectorAll(t):(console.warn("the expected parent element is not here"),null)}static getClosestHtmlElementByQuery(e,t){return e?e.closest(t):(console.warn("the expected parent element is not here"),null)}static getHtmlElement(e){const t=e.nativeElement;return t||(console.warn("the expected element is not here"),null)}static getHtmlElements(e){const t=Array.from(e);return t?t.length?t:(console.warn("the expected number of element children is not here"),[]):(console.warn("the expected element children are not here"),[])}static getHtmlHeadingElement(e=0,t=document){if(!t)return null;return 0<e&&e<7?t.createElement(`h${e}`):t.createElement("h2")}static getStyleDeclaration(e){if(!e)return null;const t=e.style;return t||(console.warn("the expected CSS style declaration is not here"),null)}static parseAsHtmlElement(e,t="div"){if(!e)return null;const r=d.parseAsHtmlElements(e,t);return r.length?r[0]:null}static parseAsHtmlElements(e,t){if(!e)return[];const r=(new DOMParser).parseFromString(e,"text/xml").getElementsByTagName(t);return Array.from(r).map((e=>e))}static timeout(e){return new Promise((t=>setTimeout(t,e)))}}class m{static getRandom(e,t){return Math.floor(Math.random()*(t-e+1))+e}static isNumeric(e){return null!==e&&(void 0!==e&&!isNaN(Number(e.toString())))}static round(e,t){let r=parseFloat(`${e}e${t}`);return r=parseFloat(`${Math.round(r)}e-${t}`),r}}Object.create;Object.create;class g{static lowerCasePropertyChar(e,t=0){if(!e)return e;return Object.keys(e).forEach((r=>{var n;e=g.renameProperty(r,`${(n=r).charAt(t).toLowerCase()}${n.slice(t+1)}`,e)})),e}static renameProperty(e,t,r){var n=e,s=r[n],o=function(e,t){var r={};for(var n in e)Object.prototype.hasOwnProperty.call(e,n)&&t.indexOf(n)<0&&(r[n]=e[n]);if(null!=e&&"function"==typeof Object.getOwnPropertySymbols){var s=0;for(n=Object.getOwnPropertySymbols(e);s<n.length;s++)t.indexOf(n[s])<0&&Object.prototype.propertyIsEnumerable.call(e,n[s])&&(r[n[s]]=e[n[s]])}return r}(r,["symbol"==typeof n?n:n+""]);return Object.assign({[t]:s},o)}}rx=t})();
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
