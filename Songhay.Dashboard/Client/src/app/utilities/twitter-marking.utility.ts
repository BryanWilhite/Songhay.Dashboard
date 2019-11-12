import { TwitterItem } from '../models/twitter-item';

export class TwitterMarkingUtility {
    static getUserUri(item: TwitterItem): string | null {
        if (!item) {
            console.error('The expected Twitter item is not here');
            return null;
        }
        if (!item.user) {
            console.error('The expected Twitter user is not here');
            return null;
        }
        if (item.user.url) { return item.user.url; }
        if (!item.user.name) {
            console.error('The expected Twitter user name is not here');
            return null;
        }
        return `https://twitter.com/${item.user.screenNameResponse}`;
    }

    static getMarkdown(i: TwitterItem): string {
        const alt = `${i.user.name} [${i.user.screenNameResponse}]`;
        const userUri = TwitterMarkingUtility.getUserUri(i);

        return [
            `<div class="tweet" data-status-id="${i.statusID}">`,
            '',
            `[<img alt="${alt}" src="${i.profileImageUrl}" />](${userUri})`,
            `${TwitterMarkingUtility.getMarkdownLinks(i.fullText || i.text)}`,
            '',
            '</div>'
        ].join('\n');
    }

    static getMarkdownLinks(tweet: string): string {
        const reForHtmlLiteral = /((https?|ftp|file):\/\/[\-A-Z0-9+&@@#\/%?=~_|!:,.;]*[\-A-Z0-9+&@@#\/%=~_|])/gi;
        const reForHandle = /[@@]+[A-Za-z0-9-_]+/g;
        const reForHashTag = /[#]+[A-Za-z0-9-_]+/g;

        tweet = tweet
            .replace(reForHtmlLiteral, '[$1]($1)')
            .replace(reForHandle, s => {
                const username = s.replace('@@', '');
                return `[${s}](https://twitter.com/${username})`;
            })
            .replace(reForHashTag, s => {
                const tag = s.replace('#', '%23');
                return `[${s}](https://twitter.com/search?q='${tag}')`;
            });

        return tweet;
    }

    static getMarkup(i: TwitterItem): string {
        const alt = `${i.user.name} [${i.user.screenNameResponse}]`;
        const userUri = TwitterMarkingUtility.getUserUri(i);

        return [
            `<div class="tweet" data-status-id="${i.statusID}">`,
            `    <a href="${userUri}" target="_blank">`,
            `        <img`,
            `            alt="${alt}]"`,
            `            src="${i.profileImageUrl}" />`,
            '    </a>',
            `    ${TwitterMarkingUtility.getMarkupLinks(i.fullText || i.text)}`,
            '</div>'
        ].join('\n');
    }

    static getMarkupLinks(tweet: string): string {
        const reForHtmlLiteral = /((https?|ftp|file):\/\/[\-A-Z0-9+&@@#\/%?=~_|!:,.;]*[\-A-Z0-9+&@@#\/%=~_|])/gi;
        const reForHandle = /[@@]+[A-Za-z0-9-_]+/g;
        const reForHashTag = /[#]+[A-Za-z0-9-_]+/g;

        tweet = tweet
            .replace(reForHtmlLiteral, '<a href="$1" target="_blank">$1</a>')
            .replace(reForHandle, s => {
                const username = s.replace('@@', '');
                return `<a href="https://twitter.com/${username}" target="_blank">${s}</a>`;
            })
            .replace(reForHashTag, s => {
                const tag = s.replace('#', '%23');
                return `<a href="https://twitter.com/search?q='${tag}" target="_blank">${s}</a>`;
            });

        return tweet;
    }
}
