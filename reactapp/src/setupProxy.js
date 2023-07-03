const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/hardprocessingsimulator",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:7014',
        secure: false
    });

    app.use(appProxy);
};
