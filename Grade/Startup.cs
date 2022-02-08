using Grade.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Grade
{
    public class Startup
    {
        private const string ConnectionKey = "PostgresConnection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GradeContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString(ConnectionKey))
            );
            services.AddDatabaseDeveloperPageExceptionFilter(); //Exibir erros
            

            //Exigindo autorização para acessar arquivos
            /*services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

            });*/
        }

        
    }
}
