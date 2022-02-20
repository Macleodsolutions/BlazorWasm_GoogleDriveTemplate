using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorWasm_GoogleDriveTemplate;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CustomAuthenticationMessageHandler>();
builder.Services.AddHttpClient("auth", opt => { opt.BaseAddress = new Uri("https://www.googleapis.com"); })
    .AddHttpMessageHandler<CustomAuthenticationMessageHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("auth"));
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
    //options.ProviderOptions.DefaultScopes.Add("https://www.googleapis.com/auth/drive.appdata");
    options.ProviderOptions.DefaultScopes.Add("https://www.googleapis.com/auth/drive.file");
});

await builder.Build().RunAsync();

public class CustomAuthenticationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthenticationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager) :
        base(provider, navigationManager)
    {
        ConfigureHandler(new string[] {"https://www.googleapis.com"});
    }
}