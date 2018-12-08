const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const HtmlWebpackPlugin = require('html-webpack-plugin');

const helpers = require('./helpers');

const isProduction = process.argv.indexOf("-p") >= 0;
console.log("Bundling for " + (isProduction ? "production" : "development") + "...");

const babelOptions = {
    presets: [
        ["@babel/preset-env", {
            "targets": {
                "browsers": [
                    "last 3 versions",
                    "ie >= 9"
                ]
            },
            "modules": false
        }]
    ]
};
module.exports = {
    devtool: 'source-map',
    entry: {
        'index': './src/Client/Client.fsproj',
    },

    output: {
        path: helpers.root('dist'),
        publicPath: 'http://localhost:3000/',
        filename: '[name].js',
    },

    optimization: {
        namedModules: true
    },
    devServer: {
        contentBase: helpers.root('./public'),
        historyApiFallback: true,
        stats: 'minimal',
        port: 3000
    },
    module: {
        rules: [{
                test: /\.fs(x|proj)?$/,
                use: {
                    loader: "fable-loader",
                    options: {
                        babel: babelOptions,
                        define: isProduction ? [] : ["DEBUG"]
                    }
                }
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: babelOptions
                },
            },
            {
                test: /.(sa|sc|c)ss$/,
                use: [ 'style-loader', 'css-loader', 'sass-loader' ]
            },
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css"
        }),
        new HtmlWebpackPlugin({
            template: 'public/index.html'
        }),
    ],

};
