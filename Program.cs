using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SysDAP;
using SysDAP.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Redirige al login si el usuario no está autenticado
    options.LogoutPath = "/Identity/Account/Logout"; // Redirige al logout
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Ruta de acceso denegado si el usuario no tiene permisos
});

//builder.Services.AddControllersWithViews(options =>
//{
//    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
//});

builder.Services.AddAuthorization();
builder.Services.AddScoped<MarcacionesService>();
var app = builder.Build();

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



//app.MapRazorPages();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


//// Redirigir al Login de Identity al inicio
//app.Use(async (context, next) =>
//{
//    if (context.Request.Path == "/")
//    {
//        context.Response.Redirect("/Identity/Account/Login");
//        return;
//    }
//    await next();
//});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
