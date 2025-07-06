window.getTelegramData = function () {
    if (window.Telegram && Telegram.WebApp && Telegram.WebApp.initDataUnsafe) {
        return JSON.stringify(Telegram.WebApp.initDataUnsafe) || "";
    }
    return null;
};