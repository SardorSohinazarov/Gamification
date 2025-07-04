window.getTelegramUser = function () {
    if (window.Telegram && Telegram.WebApp && Telegram.WebApp.initDataUnsafe) {
        return Telegram.WebApp.initDataUnsafe.user;
    }
    return null;
};