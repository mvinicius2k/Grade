using Grade;
using Grade.Data;
using Grade.Models;
using Grade.Models.Dto;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


const string ConnectionKey = "PostgresConnection";

var builder = WebApplication.CreateBuilder(args);
//var startup = new Startup(builder.Configuration);

//startup.ConfigureServices(builder.Services);

builder.Services.AddMvc(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

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

var mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.CreateMap<PresenterDto, Presenter>()
       .ForMember(dest => dest.ResourceId, opt => opt.MapFrom(src => src.ImageId));

    cfg.CreateMap<Presenter, PresenterDetailsDto>()
        .ForMember(dest => dest.ImageResource, opt => opt.MapFrom(src => src.Resource));

    cfg.CreateMap<WeeklySection, WeeklySectionDetailsDto>()
        .ForMember(dest => dest.ImageResource, opt => opt.MapFrom(src => src.Resource));

    cfg.CreateMap<LooseSection, LooseSectionDetailsDto>()
        .ForMember(dest => dest.ImageResource, opt => opt.MapFrom(src => src.Resource));

    


    /*cfg.CreateMap<Presenter, PresenterDetailsDto>()
        .ForMember(dest => dest.Sections, opt => opt.MapFrom(src => ))*/

});

builder.Services.AddSingleton(mapperConfig.CreateMapper());

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