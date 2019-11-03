const path = require("path");
const webpack = require("plumpack-assets/webpack.config.js");
module.exports = webpack({
    mode: "development",
    outputPath: path.resolve(__dirname, "..", "..", "src", "PlumPack.Wallet.Web", "wwwroot", "assets")
})