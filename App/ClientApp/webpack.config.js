const webpack = require('webpack');
const HtmlWebpackPlugin = require("html-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const path = require("path");

module.exports = {
    entry: "./src/index.tsx",
    output: {
        path: path.resolve(__dirname, "build"),
        filename: "[name].[chunkhash].js",
        publicPath: "/"
    },
    resolve: {
        extensions: [".js", ".ts", ".tsx"]
    },
    module: {
        rules: [
            {
                test: [ /\.ts$/, /\.tsx$/],
                use: "ts-loader"
            },
            {
                test: /\.css$/,
                use: [MiniCssExtractPlugin.loader, "css-loader"]
            },
            {
                test: /\.(js)$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader'
                }
            },
        ]
    },
    plugins: [
        new CleanWebpackPlugin(),
        new webpack.DefinePlugin({
            // put the date and git hash together
            "process.env.REACT_APP_COMMIT_HASH": JSON.stringify(new Date().toISOString().split('T')[0] + '-' + require('child_process').execSync('git rev-parse --short HEAD').toString())
        }),
        new HtmlWebpackPlugin({
            template: "./public/index.html",
            hash: true
        }),
        new MiniCssExtractPlugin({
            filename: "css/[name].[chunkhash].css"
        }),
        {
            apply: (compiler) => {
                compiler.hooks.done.tap("DonePlugin", (stats) => {
                    //ensure that webpack exits once compile is done
                    console.log(`Compile is done! ${stats}`);
                    setTimeout(() => { process.exit(0) });
                });
            }
        }
    ],
    optimization: {
        runtimeChunk: 'single',
        splitChunks: {
            cacheGroups: {
                vendor: {
                    test: /[\\/]node_modules[\\/]/,
                    name: 'vendors',
                    chunks: 'all',
                }
            }
        }
    },
    performance: {
        maxEntrypointSize: 1024000,
        maxAssetSize: 1024000
    },
    watch: true
};