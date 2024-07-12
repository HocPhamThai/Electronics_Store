global using Electronics_Store.Shared;

global using Electronics_Store.Server.DatabaseContext;
global using Electronics_Store.Server.Services.Service4Cart_Server;
global using Electronics_Store.Server.Services.Service4Categories_Server;
global using Electronics_Store.Server.Services.Service4Products_Server;
global using Electronics_Store.Server.Services.Service4Payment_Server;
global using Electronics_Store.Server.Services.Service4Authentication_Server;
global using Electronics_Store.Server.Services.Service4ProductVarieties_Server;
global using Electronics_Store.Server.Services.Service4Orders_Server;
global using Electronics_Store.Server.Services.Service4Addresses_Server;
global using Electronics_Store.Server.Services.Service4Email_Server;

global using Electronics_Store.Shared.Responder;
global using Electronics_Store.Shared.DTO;
global using Electronics_Store.Shared.User;
global using Electronics_Store.Shared.Order;

global using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
        // add swagger services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // connect to database
        var connectionString = builder.Configuration.GetConnectionString("DbConnection");
        builder.Services.AddDbContext<ElectronicsStoreDbContext>(options => options.UseSqlServer(connectionString));

        // add business services
        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IAddressService, AddressService>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddScoped<IProductsService, ProductsService>();
        builder.Services.AddScoped<ICategoriesService, CategoriesService>();
        builder.Services.AddScoped<IProductVarietyService, ProductVarietyService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

    builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                        .GetBytes(builder.Configuration.GetSection(
                                "AppSettings:Token").Value??string.Empty
                        )
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true
                };
            });
    builder.Services.AddHttpContextAccessor();

        // add email config
        var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<Email>();
        builder.Services.AddSingleton(emailConfig);
        builder.Services.AddScoped<IEmailService, EmailService>();


var app = builder.Build();
        // call Swagger's UI
        app.UseSwaggerUI(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

        // call out swagger services
        app.UseSwagger();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();