window.getTelegramData = function () {
    if (window.Telegram && Telegram.WebApp && Telegram.WebApp.initDataUnsafe) {
        return Telegram.WebApp.initDataUnsafe;
    }
    return null;
};