using ECPAPI.Configurations;
using ECPAPI.Data;
using ECPAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ECPAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization Header Using Bearer. Enter Bearer [Space] add  your token in the text input. Example: Bearer kdskdfjsdkjksdks",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                Scheme = "Auth2",

                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<ECPDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ECPAPIConnection"));
});

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(ICategoryRepository<>), typeof(CategoryRepository<>));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });

    options.AddPolicy("AllowOnlyLocalHost", policy =>
    {
        policy.WithOrigins("https://localhost:7237").AllowAnyHeader().AllowAnyMethod();
    });

    options.AddPolicy("AllowOnlyGoogle", policy =>
    {
        policy.WithOrigins("http://google.com", "http://gmail.com", "http://drive.google.com").AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowOnlyMicrosoft", policy =>
    {
        policy.WithOrigins("http://outlook.com", "http://microsoft.com", "http://onedrive.google.com");
    });
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200/").AllowAnyHeader().AllowAnyMethod();
    });
});

byte[] key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretForLocal"));
string localAudience = builder.Configuration.GetValue<string>("LocalAudience");
string localIssuer = builder.Configuration.GetValue<string>("LocalIssuer");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("LoginForLocalUsers",options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = localIssuer,
        ValidateAudience = true,
        ValidAudience = localAudience
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("api/testingendpoint",
        context => context.Response.WriteAsync("Test Response"))
    .RequireCors("AllowOnlyLocalHost");

    endpoints.MapControllers()
    .RequireCors("AllowAll");

    endpoints.MapGet("api/testedendpoint2",
        context => context.Response.WriteAsync("Test Response 2"));
});

app.Run();


//PRODUCTS:
//GET / api / products
//GET / api / products /{ id}
//POST / api / products
//PUT / api / products /{ id}
//DELETE / api / products /{ id}

//CATEGORIES:
//GET / api / categories
//POST / api / categories
//PUT / api / categories /{ id}   <<<<<<<<<<<<<<<<<
//DELETE / api / categories /{ id}

//ORDERS:
//GET / api / orders
//GET / api / orders /{ id}
//POST / api / orders
//PUT / api / orders /{ id}/ status

//CART:
//GET / api / cart
//POST / api / cart / items
//DELETE / api / cart / items /{ id}
//PUT / api / cart / items /{ id}/ quantity

//USERS:
//GET / api / users
//PUT / api / users /{ id}
//GET / api / users /{ id}/ orders