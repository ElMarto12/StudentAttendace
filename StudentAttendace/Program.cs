using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StudentAttendace.Config;
using StudentAttendace.Hubs;
using StudentAttendace.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5208");

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException()));

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://192.168.1.150:5208");
});


builder.Services.AddScoped<StudentService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new StudentService(httpClientFactory.CreateClient("ApiClient"));
});

builder.Services.AddScoped<TeacherService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new TeacherService(httpClientFactory.CreateClient("ApiClient"));
});

builder.Services.AddScoped<GroupService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new GroupService(httpClientFactory.CreateClient("ApiClient"));
});

builder.Services.AddScoped<SubjectService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new SubjectService(httpClientFactory.CreateClient("ApiClient"));
});

builder.Services.AddScoped<LectureService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new LectureService(httpClientFactory.CreateClient("ApiClient"));
});

builder.Services.AddScoped<UserService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new UserService(httpClientFactory.CreateClient("ApiClient"));
    
});

builder.Services.AddScoped<PdfReportService>();

builder.Services.AddAuthentication(options => {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
    });

builder.Services.AddSignalR();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapHub<AttendanceHub>("/attendanceHub");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}");

app.Run();