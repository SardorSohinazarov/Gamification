using Gamification.Student.UI;
using Gamification.Student.UI.Services.Quiz;
using Gamification.Student.UI.Services.Tests;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<ITestService, TestService>(
    client => client.BaseAddress = new Uri("https://gamification-production-b1e5.up.railway.app"));
builder.Services.AddHttpClient<IQuizService, QuizService>(
    client => client.BaseAddress = new Uri("https://gamification-production-b1e5.up.railway.app"));

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
