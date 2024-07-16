

using APICatalogo.Extensions;
using CatalogoApi.ExtensaoErros.Filtros;
using CatalogoApi.Repository;
using Dominio.Interfaces;
using Dominio.Interfaces.Generic;
using Dominio.Services.Token;
using Infraestrutura.Data;
using Infraestrutura.IdentityModel;
using Infraestrutura.Profiles.CategoriasProfile;
using Infraestrutura.Repository.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(typeof(ApiExceptionFilter));
})
    .AddJsonOptions(op => op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Barbearia", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
});

//DbContext
var stringConexao = builder.Configuration.GetConnectionString("StringDefault");
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseSqlServer(stringConexao));
builder.Services.AddDbContext<ApiDbContextIdentity>(opt => opt.UseSqlServer(stringConexao));

//Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApiDbContextIdentity>()
    .AddDefaultTokenProviders();

//Jwt Token
var secretKey = builder.Configuration["Jwt:SecretKey"] ?? throw new ArgumentException("Invalid secret Key ..");

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // desafio de solicitar o token
}).AddJwtBearer(opt =>
{
    opt.SaveToken = true; // salvar o token
    opt.RequireHttpsMetadata = true; // se é preciso https para transmitir o token , em produçao é true
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))

    };
});

//Politicas que serão usadas para acessar os endpoints
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

    opt.AddPolicy("SuperAdminOnly", policy => policy.RequireClaim("id", "fabio"));

    opt.AddPolicy("UserOnly", policy => policy.RequireRole("User"));

    opt.AddPolicy("ExcuisivePolicyOnly", policy => policy.RequireAssertion(contex => contex.User
                                                .HasClaim(claim => claim.Type == "id" && claim.Value == "fabio"
                                                      || contex.User.IsInRole("SuperAdmin"))));
}
);


//Inje��o de depend�ncia
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IUnitToWork, UnitToWork>();
builder.Services.AddScoped<ITokenService, TokenService>();



//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();


app.UseAuthorization();     

app.MapControllers();

app.Run();
