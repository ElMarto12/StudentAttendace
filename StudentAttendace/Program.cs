using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StudentAttendace.Config;
using StudentAttendace.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException()));
builder.Services.AddHttpClient<GroupService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5208");
});
builder.Services.AddHttpClient<TeacherService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5208");
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}");

app.Run();