using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PeakHours_Blazor;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using PeakHours_Blazor.Interfaces;
using PeakHours_Blazor.Helpers;
using PeakHours_Blazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7138") });

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IWorkHourService, WorkHourService>();
builder.Services.AddScoped<IProgHourService, ProgHourService>();
builder.Services.AddScoped<BeamAuthenticationStateProviderHelper>();
builder.Services.AddScoped<IUserService, AccountUserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<AuthenticationStateProvider, BeamAuthenticationStateProviderHelper>();
builder.Services.AddSweetAlert2();
builder.Services.AddScoped<LocalStorageHelper>();
builder.Services.BuildServiceProvider().GetRequiredService<IJSRuntime>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<RadzenRequiredValidator>();



var host = builder.Build();

SweetAlertHelper.Initialize((IJSRuntime)host.Services.GetService(typeof(IJSRuntime)));

await builder.Build().RunAsync();
