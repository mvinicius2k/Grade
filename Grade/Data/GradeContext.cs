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
        public DbSet<Section> ProgramsBase { get; set; }
        public DbSet<WeeklySection> WeeklyPrograms { get; set; }
        public DbSet<LooseSection> LoosePrograms { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            
            mb.Entity<Resource>().ToTable(nameof(Resource));
            mb.Entity<Section>().ToTable(nameof(Section));
            mb.Entity<LooseSection>().ToTable(nameof(LooseSection));
            mb.Entity<WeeklySection>().ToTable(nameof(WeeklySection));
            mb.Entity<Apresentation>().ToTable(nameof(Apresentation));
            mb.Entity<Presenter>().ToTable(nameof(Presenter))
                .HasMany(x => x.Apresentations)
                .WithOne(x => x.Presenter)
                .OnDelete(DeleteBehavior.Cascade);




        }

       
        

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql(Credentials);*/
        
    }
}
