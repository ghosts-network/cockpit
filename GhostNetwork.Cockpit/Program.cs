using GhostNetwork.Cockpit.Pages;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;
// Add services to the container.

builder.Services.AddScoped<NewsFeedService>();
builder.Services.AddHttpClient<NewsFeedService>(client => client.BaseAddress = new Uri(builder.Configuration["NEWSFEED_ADDRESS"]));

builder.Services.AddScoped<ProfilesService>();
builder.Services.AddHttpClient<ProfilesService>(client => client.BaseAddress = new Uri(builder.Configuration["PROFILES_ADDRESS"]));
builder.Services.AddRazorPages();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["AUTH_AUTHORITY"];

        options.ClientId = builder.Configuration["AUTH_CLIENT_ID"];
        options.ClientSecret = builder.Configuration["AUTH_CLIENT_SECRET"];
        options.ResponseType = "code";

        options.SaveTokens = true;
    });

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages()
        .RequireAuthorization();
});

app.Run();