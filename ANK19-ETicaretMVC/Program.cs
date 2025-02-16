using ANK19_ETicaretMVC.Areas.Admin.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS politikas�n� tan�ml�yoruz
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // T�m domain'lere izin veriyoruz
              .AllowAnyMethod() // T�m HTTP metodlar�na izin veriyoruz (GET, POST, vb.)
              .AllowAnyHeader(); // T�m ba�l�klara izin veriyoruz
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

// CORS politikalar�n� uyguluyoruz
app.UseCors();  // Bu, tan�mlad���n�z default policy'yi uygulayacakt�r

//*Global Exception Middleware (Canl�ya almadan �nce buras� yorum sat�r�ndan ��kar�lacak �imdilik hatalar� g�rebilmemiz i�in kapal�)
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
