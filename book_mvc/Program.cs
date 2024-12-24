using book_mvc.Contexts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Session için gerekli yapılandırma
builder.Services.AddDistributedMemoryCache(); // Session cache için
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
    options.Cookie.HttpOnly = true;  // Cookie'yi sadece HTTP üzerinden erişilebilir yapar
    options.Cookie.IsEssential = true; // Cookie'nin zorunlu olmasını sağlar
});

// PostgreSQL'e bağlanmak için DbContext ayarları
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// JSON ayarları
JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    TypeNameHandling = TypeNameHandling.Auto,
    MetadataPropertyHandling = MetadataPropertyHandling.Ignore
};

// MVC controller'ları ve görünümleri ekleyin
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Hata yönetimi ve ortam yapılandırması
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS (HTTP Strict Transport Security)
}

app.UseHttpsRedirection(); // HTTPS'e yönlendirme
app.UseStaticFiles(); // Statik dosyaların sunulması (CSS, JS, vb.)

// Session yönetimini etkinleştir
app.UseSession();

// Uygulama yönlendirmesi ve yetkilendirme
app.UseRouting();
app.UseAuthorization();

// Controller ve Action yönlendirmeleri
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");

app.Run();
