using ANK19_ETicaretMVC.Areas.Admin.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS politikasýný tanýmlýyoruz
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // Tüm domain'lere izin veriyoruz
              .AllowAnyMethod() // Tüm HTTP metodlarýna izin veriyoruz (GET, POST, vb.)
              .AllowAnyHeader(); // Tüm baþlýklara izin veriyoruz
    });
});

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<HttpClient>();

builder.Services.AddDbContext<SchafoldIcinDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSession();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

// CORS politikalarýný uyguluyoruz
app.UseCors();  // Bu, tanýmladýðýnýz default policy'yi uygulayacaktýr

//*Global Exception Middleware (Canlýya almadan önce burasý yorum satýrýndan çýkarýlacak þimdilik hatalarý görebilmemiz için kapalý)
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.Redirect("/Error");
    }
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
