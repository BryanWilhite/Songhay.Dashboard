const path = require('path');

module.exports = {
    entry: {
        rx: [
            path.resolve(__dirname, 'node_modules/songhay/dist/songhay.js'),
            path.resolve(__dirname, 'src/js/site.js')
        ]
    },
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, 'wwwroot/js'),
    },
    mode: 'production'
};
