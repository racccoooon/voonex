using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Quartz;
using voonex.api.Models;
using voonex.api.Services;
using voonex.api.Services.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<VoonexDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("voonex"), optionsBuilder =>
    {
        optionsBuilder.UseNodaTime();
    });
});

builder.Services.AddQuartz(configurator =>
{
    // configuration voodoo
    configurator.InterruptJobsOnShutdown = true;
    configurator.InterruptJobsOnShutdownWithWait = true;
    configurator.UseMicrosoftDependencyInjectionJobFactory();
    configurator.ScheduleJob<SessionCleanUp>(trigger =>
        trigger.WithSimpleSchedule(x => x.WithIntervalInHours(12)).StartNow());
});
builder.Services.AddQuartzServer(options =>
{
    options.WaitForJobsToComplete = false;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
});
builder.Services.AddScoped<UserInfo>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
var dbContextFactory = app.Services.GetRequiredService<IDbContextFactory<VoonexDbContext>>();
await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
{
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();