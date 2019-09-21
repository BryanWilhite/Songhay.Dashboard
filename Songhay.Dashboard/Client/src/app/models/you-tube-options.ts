import {
    YouTubeOptions,
    YouTubeCssOptionUtility
} from '@songhay/player-video-you-tube';

export const YOU_TUBE_OPTIONS: YouTubeOptions = {
    youTubeCssOptions: YouTubeCssOptionUtility
        .getDefaultOptions()
        .map(i => {
            switch (i.variableName) {
                case '--thumbs-header-link-color':
                    return {
                        variableName: i.variableName,
                        variableValue: '#6eff93'
                    };

                case '--thumbs-set-header-color':
                    return {
                        variableName: i.variableName,
                        variableValue: '#6eff93'
                    };

                case '--thumbs-set-header-position':
                    return {
                        variableName: i.variableName,
                        variableValue: 'static'
                    };

                case '--thumbs-set-padding-top':
                    return {
                        variableName: i.variableName,
                        variableValue: '0'
                    };

                default:
                    return i;
            }
        }),
    youTubeSpritesUri: 'assets/svg/sprites.svg'
};
