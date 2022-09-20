using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.DAL;
using WebAPI.apiauthorization;
using WebAPI.mapping;
using WebAPI.services;
using FluentValidation.AspNetCore;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var env = builder.Environment;
var config = builder.Configuration;

{
    services.AddControllers();
    services.AddCors();
    services.AddLogging();

    // configure automapper with all automapper profiles from this assembly
    //services.AddAutoMapper(typeof(Program));

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

    IMapper mapper = mapperConfig.CreateMapper();
    services.AddSingleton(mapper);

    var connStr = config.GetSection("ConnectionStrings").GetSection("MainDB").Value;

    builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connStr));
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    //services.Configure<AppSettings>(config.GetSection("AppSettings"));

    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IBusinessService, BusinessService>();
    services.AddScoped<ICountryService, CountryService>();
    services.AddScoped<IAccessService, AccessService>();
    services.AddControllersWithViews().AddFluentValidation();
    //Commmented by rishi
    //services.AddScoped<IUserService, UserService>();
    //services.AddScoped<IJwtUtils, JwtUtils>();

}

var app = builder.Build();

{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    // configure HTTP request pipeline
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // global error handler
    //app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    //app.UseMiddleware<JwtMiddleware>();    
}

app.Run(); //"https://localhost:7108/api/user/version"
