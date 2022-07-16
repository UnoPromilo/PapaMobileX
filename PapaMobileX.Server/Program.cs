using PapaMobileX.Server.BusinessLogic;
using PapaMobileX.Server.Camera;
using PapaMobileX.Server.Configuration;
using PapaMobileX.Server.DataSource;
using PapaMobileX.Server.Mappers;
using PapaMobileX.Server.Security;
using PapaMobileX.Server.SignalR.Hubs;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services
       .AddLogging()
       .ConfigureSwagger()
       .ConfigureMappers()
       .ConfigureBusinessLogic()
       .ConfigureSecurity(builder.Configuration)
       .ConfigureDatasource(builder.Configuration)
       .ConfigureCamera()
       .AddEndpointsApiExplorer()
       .AddControllers();

WebApplication app = builder.Build();

app.UseSwagger(SwaggerConfiguration.ConfigureSwaggerUsage);

if (app.Environment.IsDevelopment())
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ControlHub>("/hubs/control");
app.UseWebSockets();

app.Run();