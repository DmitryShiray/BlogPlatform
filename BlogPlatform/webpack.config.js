module.exports = function (env) {
    return require(`./Config/webpack.${env}.js`);
};