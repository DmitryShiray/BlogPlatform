const path = require('path');

const webpack = require('webpack');

const HtmlWebpackPlugin = require('html-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');

const helpers = require('./webpack.helpers');

const ROOT = path.resolve(__dirname, '..');

module.exports = {
    devtool: 'source-map',

    performance: {
        hints: false
    },

    entry: {
        'polyfills': './ClientApp/dist/polyfills.ts',
        'vendor': './ClientApp/dist/vendor.ts',
        'app': './ClientApp/app/main.ts'
    },

    output: {
        path: ROOT + '/wwwroot/dist/',
        filename: '[name].bundle.js',
        chunkFilename: '[id].chunk.js',
        publicPath: '/dist/'
    },

    resolve: {
        extensions: ['.ts', '.js', '.json']
    },

    devServer: {
        historyApiFallback: true,
        hot: true,
        contentBase: path.join(ROOT, '/wwwroot/'),
        watchOptions: {
            aggregateTimeout: 300,
            poll: 1000
        }
    },

    module: {
        rules: [
            {
                test: /\.ts$/,
                use: [
                    'awesome-typescript-loader',
                    'angular-router-loader',
                    'angular2-template-loader',
                    'source-map-loader'
                ]
            },
            {
                test: /\.(png|jpg|gif|woff|woff2|ttf|svg|eot)$/,
                use: 'file-loader?name=assets/[name]-[hash:6].[ext]'
            },
            {
                test: /favicon.ico$/,
                use: 'file-loader?name=/[name].[ext]'
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
                test: /\.html$/,
                use: 'raw-loader',
            }
        ],
        exprContextCritical: false
    },

    plugins: [
        new webpack.optimize.CommonsChunkPlugin(
            {
                name: ['vendor', 'polyfills']
            }),

        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: JSON.stringify('development')
            }
        }),

        new CleanWebpackPlugin(
            [
                './wwwroot/dist',
                './wwwroot/assets'
            ],
            { root: ROOT }
        ),

        new HtmlWebpackPlugin({
            filename: '../Views/Home/Index.cshtml',
            inject: 'body',
            template: 'ClientApp/Index.cshtml'
        })
    ]
};

