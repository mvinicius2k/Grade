using Grade;
using Grade.Controllers;
using Grade.Converters;
using Grade.Data;
using Grade.Models;
using Grade.Models.Dto;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


const string ConnectionKey = "PostgresConnection";
const string SwaggerVersion = "v1";
const string SwaggerTitle = "API da Grade de Programação.";
const string TimeOnlyConverterPattern = "HH:mm";


var builder = WebApplication.CreateBuilder(args);
//var startup = new Startup(builder.Configuration);

//startup.ConfigureServices(builder.Services);

builder.Services.AddMvc(options =>
{
    //options.Filters.Add(new ValidateAntiForgeryTokenAttribute());
    options.Filters.Add(new IgnoreAntiforgeryTokenAttribute());
});

builder.Services.AddDbContext<GradeContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString(ConnectionKey))
            );
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); //Exibir erros

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter(TimeOnlyConverterPattern));
    
}); 


 builder.Services.AddSwaggerGen(g =>
{
    g.SwaggerDoc(SwaggerVersion, new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = SwaggerVersion,
        Title = SwaggerTitle,
      
    });

});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = AntiForgeryController.AntiforgeryKeyHeader;
});

var mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.CreateMap<PresenterDto, Presenter>()
       .ForMember(dest => dest.ResourceId, opt => opt.MapFrom(src => src.ImageId))
       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

    cfg.CreateMap<Presenter, PresenterDetailsDto>()
        .ForMember(dest => dest.ImageResource, opt => opt.MapFrom(src => src.Resource));
        

    cfg.CreateMap<Section, WeeklySectionDetailsDto>()
        .ForMember(dest => dest.ImageResource, opt => opt.MapFrom(src => src.Resource));

    cfg.CreateMap<Section, LooseSectionDetailsDto>()
        .ForMember(dest => dest.ImageResource, opt => opt.MapFrom(src => src.Resource));

    cfg.CreateMap<Apresentation, SectionDto>()
    .ConvertUsing((apresentation, _, context) =>
    {
        return context.Mapper.Map<Section, SectionDto>(apresentation.Section);
        

    });

    cfg.CreateMap<Section, SectionDto>();
    cfg.CreateMap<WeeklySection, WeeklySectionDetailsDto>()
        .IncludeBase<Section, SectionDto>();
    cfg.CreateMap<LooseSection, LooseSectionDetailsDto>()
       .IncludeBase<Section, SectionDto>();


});
builder.Services.AddSingleton(mapperConfig.CreateMapper());

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