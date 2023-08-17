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
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<WeeklySection> WeeklySections { get; set; }
        public DbSet<LooseSection> LooseSections { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            
            mb.Entity<Resource>().ToTable(nameof(Resource));
            mb.Entity<Section>().ToTable(nameof(Section))
                .HasMany(x => x.Presentations)
                .WithOne(x => x.Section)
                .OnDelete(DeleteBehavior.Cascade);
            mb.Entity<LooseSection>().ToTable(nameof(LooseSection));
            mb.Entity<WeeklySection>().ToTable(nameof(WeeklySection))
                .HasMany(x => x.Presentations)
                .WithOne(x => x.Section as WeeklySection)
                .OnDelete(DeleteBehavior.Cascade); ;
            mb.Entity<Presentation>().ToTable(nameof(Presentation));
            mb.Entity<Presenter>().ToTable(nameof(Presenter))
                .HasMany(x => x.Presentations)
                .WithOne(x => x.Presenter)
                .OnDelete(DeleteBehavior.Cascade);




        }

       
        

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql(Credentials);*/
        
    }
}
