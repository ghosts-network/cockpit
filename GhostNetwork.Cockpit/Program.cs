using GhostNetwork.Cockpit.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;
// Add services to the container.

builder.Services.AddScoped<NewsFeedService>();
builder.Services.AddHttpClient<NewsFeedService>(client => client.BaseAddress = new Uri(builder.Configuration["NEWSFEED_ADDRESS"]));

builder.Services.AddScoped<ProfilesService>();
builder.Services.AddHttpClient<ProfilesService>(client => client.BaseAddress = new Uri(builder.Configuration["PROFILES_ADDRESS"]));
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToPage("/AccessDenied");
});
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies", options =>
    {
        options.AccessDeniedPath = "/AccessDenied";
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["AUTH_AUTHORITY"];
        options.Scope.Add("roles");

        options.ClientId = builder.Configuration["AUTH_CLIENT_ID"];
        options.ClientSecret = builder.Configuration["AUTH_CLIENT_SECRET"];
        options.ResponseType = "code";

        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClaimActions.MapJsonKey("role", "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

        options.Events.OnRedirectToIdentityProvider = context =>
        {
            var forwardedHost = context.Request.Headers["X-Forwarded-Host"].ToString();
            if (!string.IsNullOrEmpty(forwardedHost))
            {
                context.ProtocolMessage.RedirectUri = "https://cockpit.ghost-network.boberneprotiv.com/signin-oidc";
            }

            return Task.CompletedTask;
        };

        options.SaveTokens = true;
    });

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy("CockpitMinimumAccess",
            policyBuilder => policyBuilder.RequireClaim("role", "admin"));
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
        .RequireAuthorization("CockpitMinimumAccess");
});

app.Run();