using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using test01;
using test01.Authentication;
using test01.Data;
using test01.Filters;
using test01.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("config.json");

////First mothed for  Register the AttachmentsOptions from the configuration
//var attachmentsOptions = builder.Configuration.GetSection("Attachments").Get<AttachmentsOptions>();
//// Register the AttachmentsOptions with the DI container
//builder.Services.AddSingleton(attachmentsOptions);

//////second  mothed for  Register the AttachmentsOptions from the configuration
//var attachmentsOptions = new AttachmentsOptions();
//// Bind the configuration section to the AttachmentsOptions instance
//builder.Configuration.GetSection("Attachments").Bind(attachmentsOptions);
//// Register the AttachmentsOptions with the DI container
//builder.Services.AddSingleton(attachmentsOptions);

// Register the AttachmentsOptions using IOptions pattern
builder.Services.Configure<AttachmentsOptions>(builder.Configuration.GetSection("Attachments"));


// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers(Options => Options.Filters.Add<LogActivtiyFilter>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dependency injection
builder.Services.AddDbContext<ApplicationDbContext>( options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions,BasicAuthenticationHandler>("Basic", null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<RateLimitingMiddleware>();

app.UseMiddleware<ProfilingMiddIeware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
