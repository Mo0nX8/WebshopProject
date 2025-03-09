using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using Webshop.Authenticator.Services.Authenticator;
using Webshop.Authenticator.Services.Encrypt;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Data;
using Webshop.EntityFramework.Managers.Implementations;
using Webshop.EntityFramework.Managers.Interfaces;
using Webshop.EntityFramework.Managers.Interfaces.Cart;
using Webshop.EntityFramework.Managers.Interfaces.Carts;
using Webshop.EntityFramework.Managers.Interfaces.Order;
using Webshop.EntityFramework.Managers.Interfaces.Product;
using Webshop.EntityFramework.Managers.Interfaces.User;
using Webshop.Services.Interfaces_For_Services;
using Webshop.Services.Services.Validators.Implementations;
using WebshopWeb.Initializer;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy=JsonNamingPolicy.CamelCase;
    });

builder.Services.AddDbContext<GlobalDbContext>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    
    options.IdleTimeout = TimeSpan.FromMinutes(180);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthenticationManager, AuthenticatorService>();
builder.Services.AddScoped<IEncryptManager, SHA256Encrypter>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderManager, OrderManager>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartManager, CartManager>();
builder.Services.AddScoped<IProductManager, ProductManager>();
builder.Services.AddScoped<EmailValidator, EmailValidator>();
builder.Services.AddScoped<PasswordValidator, PasswordValidator>();
builder.Services.AddScoped<UsernameValidator , UsernameValidator>();
builder.Services.AddScoped<IValidationManager, EmailValidator>();
builder.Services.AddScoped<IValidationManager, PasswordValidator>();
builder.Services.AddScoped<IValidationManager, UsernameValidator>();

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

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using(var scope=app.Services.CreateScope())
{
    var scopedServices=scope.ServiceProvider;
    var configuration=scopedServices.GetRequiredService<IConfiguration>();
    var userManager=scopedServices.GetRequiredService<IUserManager>();
    var encryptManager=scopedServices.GetRequiredService<IEncryptManager>();
    var _context = scopedServices.GetRequiredService<GlobalDbContext>();
    DbInitializer.Seed(app,userManager,encryptManager,_context, Path.Combine(Directory.GetCurrentDirectory(), "Data", "products.json"),configuration);
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
            name: "productDetails",
            pattern: "Product/Details/{name}/{id}",
            defaults: new { controller = "Product", action = "Details" });
});

app.Run();
