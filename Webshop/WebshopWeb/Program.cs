using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Text.Json;
using Webshop.EntityFramework;
using Webshop.EntityFramework.Managers.Carts;
using Webshop.EntityFramework.Managers.Order;
using Webshop.EntityFramework.Managers.Product;
using Webshop.EntityFramework.Managers.Reviews;
using Webshop.EntityFramework.Managers.User;
using Webshop.Services.Interfaces;
using Webshop.Services.Services.Authentication;
using Webshop.Services.Services.Compatibility;
using Webshop.Services.Services.Email;
using Webshop.Services.Services.OrderService;
using Webshop.Services.Services.ProductService;
using Webshop.Services.Services.Security;
using Webshop.Services.Services.Validators;
using WebshopWeb.Initializer;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy=JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); 
    options.SlidingExpiration = true; 
    options.Cookie.HttpOnly = true; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
})
.AddGoogle(options =>
{
    options.ClientId = "1095549586232-ujkvserq3oscjpkm41dtigv96mnm27vo.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.CallbackPath = "/signin-google";
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
builder.Services.AddScoped<IAuthenticationManager, AuthenticationService>();
builder.Services.AddScoped<IEncryptManager, SHA256Encrypter>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderManager, OrderManager>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartManager, CartManager>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductManager, ProductManager>();
builder.Services.AddScoped<EmailValidator, EmailValidator>();
builder.Services.AddScoped<PasswordValidator, PasswordValidator>();
builder.Services.AddScoped<UsernameValidator , UsernameValidator>();
builder.Services.AddScoped<IValidationManager, EmailValidator>();
builder.Services.AddScoped<IValidationManager, PasswordValidator>();
builder.Services.AddScoped<IValidationManager, UsernameValidator>();

builder.Services.AddScoped<IProductServices, ProductService>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewManager, ReviewManager>();
builder.Services.AddScoped<ICompatibilityService, CompatibilityService>();
builder.Services.AddScoped<IEmailService,EmailService>();

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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

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

app.MapControllerRoute(
    name: "productDetails",
    pattern: "Termekek/{name}/{id}",
    defaults: new { controller = "Product", action = "Details" });

app.Run();
