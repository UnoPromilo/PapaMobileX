using PapaMobileX.Server.BusinessLogic;
using PapaMobileX.Server.Configuration;
using PapaMobileX.Server.DataSource;
using PapaMobileX.Server.Mappers;
using PapaMobileX.Server.Security;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services
    .ConfigureSwagger()
    .ConfigureMappers()
    .ConfigureBusinessLogic()
    .ConfigureSecurity(builder.Configuration)
    .ConfigureDatasource(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddControllers();

WebApplication app = builder.Build();

//Definitions for proxy
app.UseSwagger(SwaggerConfiguration.ConfigureSwaggerUsage);

if (app.Environment.IsDevelopment())
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();