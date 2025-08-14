using System.Text.Json.Serialization;
using DIT.Test_Title_Management_Service.Application.Features;
using DIT.Test_Title_Management_Service.Host.Extensions;
using DIT.Test_Title_Management_Service.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services
    .AddProblemDetails()
    .AddEndpointsApiExplorer()
    .RegisterSwaggerServices()
    .AddApplicationFeatures()
    .AddPersistence(builder.Configuration);

builder.ApplyFluentResultsConfiguration();
var app = builder.Build();

app.UseSwaggerService();
app.Services.MigrateDatabase();
app.MapControllers();

app.Run();