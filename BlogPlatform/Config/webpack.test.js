﻿const path = require('path');

module.exports = {

    devtool: 'inline-source-map',

    performance: {
        hints: false
    },

    resolve: {
        extensions: ['.ts', '.js']
    },

    module: {
        rules: [
            {
                test: /\.ts$/,
                use: [
                    'awesome-typescript-loader',
                    'angular2-template-loader',
                    'source-map-loader'
                ]
            },
            {
                test: /\.css$/,
                use: [
                    'css-to-string-loader',
                    'style-loader',
                    'css-loader'
                ]
            },
            {
                test: /\.scss$/,
                include: path.join(__dirname, 'ClientApp/styles'),
                use: [
                    'style-loader',
                    'css-loader',
                    'sass-loader'
                ]
            },
            {
                test: /\.scss$/,
                exclude: path.join(__dirname, 'ClientApp/styles'),
                use: [
                    'raw-loader',
                    'sass-loader'
                ]
            },
            {
                test: /\.html$/,
                use: 'raw-loader'
            }
        ],
        exprContextCritical: false
    }

};
