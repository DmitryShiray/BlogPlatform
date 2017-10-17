module.exports = env => {
    var environment = 'development';

    if (!env) {
        var launch = require('./Properties/launchSettings.json');

        environment = process.env.ASPNETCORE_ENVIRONMENT.toLowerCase() ||
            (launch && launch.profiles['IIS Express'].environmentVariables.ASPNETCORE_ENVIRONMENT.toLowerCase()) ||
            'development';
    }
    else
    {
        environment = env.NODE_ENV;
    }

    if (environment === 'production') {
        return require('./Config/webpack.prod.js');
    }
    else {
        return require('./Config/webpack.dev.js');
    }
};