

using APICatalogo.Extensions;
using CatalogoApi.ExtensaoErros.Filtros;
using CatalogoApi.Repository;
using Dominio.Interfaces;
using Dominio.Interfaces.Generic;
using Infraestrutura.Data;
using Infraestrutura.Profiles.CategoriasProfile;
using Infraestrutura.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSwaggerGen();

//DbContext
var stringConexao = builder.Configuration.GetConnectionString("StringDefault");
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseSqlServer(stringConexao));

//Inje��o de depend�ncia
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IUnitToWork, UnitToWork>();

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
