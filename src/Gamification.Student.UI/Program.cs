using Gamification.Student.UI;
using Gamification.Student.UI.Helpers;
using Gamification.Student.UI.Services.Auth;
using Gamification.Student.UI.Services.Quiz;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IQuizService, QuizService>(
    client => client.BaseAddress = new Uri("https://gamification-production-b1e5.up.railway.app"));
builder.Services.AddHttpClient<IAuthService, AuthService>(
    client => client.BaseAddress = new Uri("https://gamification-production-b1e5.up.railway.app"));
builder.Services.AddSingleton<TelegramUserHelper>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
