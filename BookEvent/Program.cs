using Core.Context;
using Microsoft.EntityFrameworkCore;
using System;
using Umbraco.Cms.Core.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(builder.Environment.ContentRootPath, "umbraco\\Data"));        
builder.Services.AddDbContext<BookEventDbContext>(options =>
            options.UseSqlite(@"Data Source=C:\Temp\EventPlan.sqlite.db"));

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

WebApplication app = builder.Build();

await app.BootUmbracoAsync();

app.UseHttpsRedirection();
UpdateDatabase(app);
app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();

//This will automatically initialize the database once the project runs
static void UpdateDatabase(IApplicationBuilder app)
{
    using var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();
    var context = serviceScope.ServiceProvider.GetService<BookEventDbContext>();

    if (context != null)
    {
        context.Database.Migrate();

    }
}

