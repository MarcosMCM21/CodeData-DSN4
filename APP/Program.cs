using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CodeData_Connection.Areas.Identity.Data;
using CodeData.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using CodeData_Connection.Services;
using CodeData_Connection.Middleware;
using System.Text;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => { 
        options.SignIn.RequireConfirmedAccount = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<IdentityErrorDescriberPtBr>(); // Registro do descritor customizado

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.Name = "CodeData-Connection";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;

    //ReturnUrlParameter requires 
    //using Microsoft.AspNetCore.Authentication.Cookies;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
});

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Em produção, defina como true
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JWT:SecretKey"])),

            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expira��o da sess�o
    options.Cookie.HttpOnly = true; // Mais seguran�a
    options.Cookie.IsEssential = true;
});

builder.Services.AddTransient<SendGridEmailSender>();

builder.Services.AddTransient<LockScreenMiddleware>();

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseCors("ApiCorsPolicy");

app.UseSession();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}");

    _ = endpoints.MapAreaControllerRoute(
        name: "sistemaA_default",
        areaName: "SistemaA",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    _ = endpoints.MapAreaControllerRoute(
        name: "sistemaB_default",
        areaName: "SistemaB",
        pattern: "SistemaB/{controller=Home}/{action=Index}/{id?}");

    _ = endpoints.MapControllerRoute(
            name: "api",
            pattern: "api/authentication/{action}/{id?}",
            defaults: new { controller = "Authentication" },
            constraints: new { controller = "Authentication" });

    _ = endpoints.MapRazorPages();
});

app.UseLockScreenMiddleware(); // Redireciona para a tela de LockScreen

app.Run();