using Grade.Models;
using Microsoft.EntityFrameworkCore;

namespace Grade.Data
{
    public class GradeContext : DbContext
    {
        //private const string Credentials = "Host=my_host;Database=my_db;Username=my_user;Password=my_pw";
        


        public GradeContext(DbContextOptions<GradeContext> options) : base(options)
        {

        }

        public DbSet<Presenter> Presenters { get; set; }
        public DbSet<Apresentation> Apresentations { get; set; }
        public DbSet<ProgramBase> ProgramsBase { get; set; }
        public DbSet<WeeklyProgram2> WeeklyPrograms { get; set; }
        public DbSet<LooseProgram2> LoosePrograms { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            
            mb.Entity<Resource>().ToTable(nameof(Resource));
            mb.Entity<ProgramBase>().ToTable(nameof(ProgramBase));
            mb.Entity<LooseProgram2>().ToTable(nameof(LooseProgram2));
            mb.Entity<WeeklyProgram2>().ToTable(nameof(WeeklyProgram2));
            mb.Entity<Apresentation>().ToTable(nameof(Apresentation));
            mb.Entity<Presenter>().ToTable(nameof(Presenter));




        }

       
        

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql(Credentials);*/
        
    }
}
