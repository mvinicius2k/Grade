using Grade;
using Grade.Data;
using Grade.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System.Text.Json.Serialization;
using System.Web.Http;


const string ConnectionKey = "PostgresConnection";

var builder = WebApplication.CreateBuilder(args);
//var startup = new Startup(builder.Configuration);

//startup.ConfigureServices(builder.Services);

builder.Services.AddDbContext<GradeContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionKey))
            );
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); //Exibir erros

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    
}); 


 builder.Services.AddSwaggerGen(g =>
{
    g.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "API da Grade de Programação",
    });

});

var app = builder.Build();

/*var httpConfig = new HttpConfiguration();

httpConfig.EnableSwagger(
    x => x.SingleApiVersion("v1", "API da Grade de Programação"))
    .EnableSwaggerUi();*/

    
    
    
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    


}

app.UseHttpsRedirection();

app.UseRouting();





app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );



app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("swagger/v1/swagger.json", "GradeAPI V1");
    c.RoutePrefix = string.Empty;


});


/*app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Resources")),
    RequestPath = "/resources"
})*/
;

CreateDbIfNotExists();

app.Run();



void CreateDbIfNotExists()
{
    using (var scope  = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Section>>();
        try
        {
            var context = services.GetRequiredService<GradeContext>();
            DbInitializer.Initialize(context);
            logger.LogInformation("DB inicializado com sucesso");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao criar o DB");
        }
    }
}