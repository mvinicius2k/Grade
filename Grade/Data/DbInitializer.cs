using Grade.Helpers;
using Grade.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Grade.Data
{
    public static  class DbInitializer
    {

        private const bool InsertDataTest = true;

        
        public static void Initialize(GradeContext context)
        {
            context.Database.EnsureCreated();

            using ILoggerFactory loggerFactory =
                LoggerFactory.Create(builder =>
                    builder.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                        options.TimestampFormat = "hh:mm:ss ";
                    }));

            
            var logger =  loggerFactory.CreateLogger<GradeContext>();
            if (InsertDataTest && !context.ProgramsBase.Any())
            {
                
                logger.LogInformation("DB vazio, inserindo valores teste");
                InsertDataTestToTables(context, logger);

            } else
            {
                
                logger.LogInformation("DB já contem dados");
                

            

            }
        }

        private static void InsertDataTestToTables(GradeContext context, ILogger logger)
        {
            var presenters = new Presenter[]
            {
                new Presenter{ Name = "Valdenir Rodrigues" },
                new Presenter{ Name = "Playlist"},
                new Presenter{ Name = "Equipe Esportiva"}
            };
            var programs = new ProgramBase[]
            {
                new WeeklyProgram2
                {
                    Name = "Asa Branca Esporte Clube",
                    StartDay = DayOfWeek.Monday,
                    EndDay = DayOfWeek.Friday,
                    StartAt = new TimeOnly(11,0),
                    EndAt = new TimeOnly(12,0),
                    Description = "O comando esporte de Boa Viagem e região. Um resumo do esporte amador e profissional da cidade e sertão.",
                    Active = true,
                    

                },
                new WeeklyProgram2
                {
                    Name = "Tarde Show",
                    StartDay = DayOfWeek.Monday,
                    EndDay = DayOfWeek.Friday,
                    StartAt = new TimeOnly(14,0),
                    EndAt = new TimeOnly(16,0),
                    Description = "Um programa voltado para o público jovem e focado nos grandes lançamentos que fazem as paradas de sucesso pelas rádios de o Brasil.",
                    Active = true,

                },
                new LooseProgram2
                {
                    Name = "Jornada Esportiva",
                    StartAt = new DateTime(2021, 01, 29, 18,0 ,0),
                    EndAt = new DateTime(2021, 01, 29, 20,0 ,0),
                    Description = "Transmissão ao vivo do campeonato X",
                    Active = true,
                }

            };

            
            logger.LogInformation("Inserindo apresentadores");
            foreach (var presenter in presenters)
            {
                logger.LogInformation(presenter.ToString());
                context.Presenters.Add(presenter);

            }

            logger.LogInformation("Inserindo apresentadores");
            foreach(var program in programs)
            {
                if (program is IResolveTypesPgsql)
                    ((IResolveTypesPgsql)program).ResolveTypesPgsql();

                logger.LogInformation(program.ToString());
                context.ProgramsBase.Add(program);

            }

            context.SaveChanges();

            var apresentations = new Apresentation[]
            {
                new Apresentation
                {
                    PresenterId = context.Presenters.First(x => x.Name == "Valdenir Rodrigues").Id,
                    ProgramBaseId = context.ProgramsBase.First(x => x.Name == "Asa Branca Esporte Clube").Id,
                },
                new Apresentation
                {
                    PresenterId = context.Presenters.First(x => x.Name == "Playlist").Id,
                    ProgramBaseId = context.ProgramsBase.First(x => x.Name == "Tarde Show").Id,
                },
                new Apresentation
                {
                    PresenterId = context.Presenters.First(x => x.Name == "Equipe Esportiva").Id,
                    ProgramBaseId = context.ProgramsBase.First(x => x.Name == "Jornada Esportiva").Id,
                }
            };

            logger.LogInformation("Inserindo apresentação");
            foreach(var apresentation in apresentations)
            {
                logger.LogInformation(apresentation.ToString());
                context.Apresentations.Add(apresentation);

            }
            context.SaveChanges();


        }
    }
}
