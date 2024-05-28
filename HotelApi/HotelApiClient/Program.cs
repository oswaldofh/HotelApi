using HotelApi.Domain.Repositories;
using HotelApi.Infrastructure.Data;
using HotelApi.Infrastructure.HotelApiMappers;
using HotelApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(o =>
{
    //SE OBTIENE LA CADENA DE CONEXION
    o.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSqlServer"));
});


builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IHotelTypeRepository, HotelTypeRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IMailRepository, MailRepository>();

builder.Services.AddTransient<SeedDb>();//ALIMENTADOR DE BASE DE DATOS

builder.Services.AddAutoMapper(typeof(HotelApiMapper));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath); //SE INCLUYEN COMETARIOS XML
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();   
app.Run();
