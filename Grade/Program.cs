using Grade;
using Grade.Data;
using Grade.Models;
using Microsoft.EntityFrameworkCore;

const string ConnectionKey = "PostgresConnection";

var builder = WebApplication.CreateBuilder(args);
//var startup = new Startup(builder.Configuration);

//startup.ConfigureServices(builder.Services);

builder.Services.AddDbContext<GradeContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionKey))
            );
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); //Exibir erros
builder.Services.AddControllersWithViews();


// Add services to the container.
builder.Services.AddControllersWithViews();




var app = builder.Build();



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
    pattern: "{controller=Home}/{action=Index}/{id?}");




/*app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Resources")),
    RequestPath = "/resources"
})*/;

CreateDbIfNotExists();

app.Run();



void CreateDbIfNotExists()
{
    using (var scope  = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<ProgramBase>>();
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