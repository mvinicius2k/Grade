using AutoMapper;
using Grade.Data;
using Grade.Helpers;
using Grade.Models;
using Grade.Models.Dto;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Grade.Controllers
{
    [ApiController][Route(Constants.ControllerDefaultRoute)]
    public class SectionsController : Controller
    {
        #region Constants
        private const string CreateWeeklyActionRoute = $"{Constants.CreateActionRoute}WeeklySection";
        private const string CreateLooseActionRoute = $"{Constants.CreateActionRoute}LooseSection";
        private const string EditWeeklyActionRoute = $"{Constants.EditActionRoute}WeeklySection";
        private const string EditLooseActionRoute = $"{Constants.EditActionRoute}LooseSection";
        #endregion

        private readonly GradeContext _context;
        private readonly ILogger<SectionsController> _logger;
        private readonly IMapper _mapper;

        public SectionsController(GradeContext context, ILogger<SectionsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAllWithDetails(string? sortOrder = null, bool includeInactive = false)
        {
            var weeklySections = await _context.WeeklySections
                    .Where(x => x.Active || includeInactive)
                    .Include(x => x.Apresentations)
                    .ThenInclude(x => x.Presenter)
                    .AsNoTracking()
                    .ToListAsync();

            var looseSections = await _context.LooseSections
                .Where(x => x.Active || includeInactive)
                .Include(x => x.Apresentations)
                .ThenInclude(x => x.Presenter)
                .AsNoTracking()
                .ToListAsync();



            var weeklyDto = new List<WeeklySectionDetailsDto>(weeklySections.Count);
            var looseDto = new List<LooseSectionDetailsDto>(looseSections.Count);

            foreach (var weeklySection in weeklySections)
            {
                var dto = _mapper.Map<WeeklySectionDetailsDto>(weeklySection);
                dto.Presenters = _mapper.Map<PresenterDetailsDto[]>(weeklySection.Apresentations);
                
                weeklyDto.Add(dto);
            }
            foreach (var looseSection in looseSections)
            {
                var dto = _mapper.Map<LooseSectionDetailsDto>(looseSection);
                dto.Presenters = _mapper.Map<PresenterDetailsDto[]>(looseSection.Apresentations);
                looseDto.Add(dto);
            }

            var sectionsDto = new
            {
                Weekly = new List<WeeklySectionDetailsDto>(weeklySections.Count),
                Loose = new List<LooseSectionDetailsDto>(looseSections.Count)
            };

            

            switch (sortOrder)
            {
                case "time":
                    sectionsDto.Weekly.AddRange(weeklyDto
                        .OrderBy(x => x.StartDay)
                        .ThenBy(x => x.StartAt));
                    sectionsDto.Loose.AddRange(looseDto
                        .OrderBy(x => x.StartAt));
                    break;

                default:
                    sectionsDto.Weekly.AddRange(
                        weeklyDto.OrderBy(x => x.Name));
                    sectionsDto.Loose.AddRange(
                        looseDto.OrderBy(x => x.Name));
                    break;

            }
            return Ok(sectionsDto);

        }
        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            var section = await _context.Sections
                .Include(x => x.Apresentations)
                .ThenInclude(x => x.Presenter)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (section is null)
                return NotFound();

            if(section is WeeklySection)
            {
                var dto =_mapper.Map<WeeklySectionDetailsDto>(section);
                dto.Presenters = _mapper.Map<PresenterDetailsDto[]>(section.Apresentations);
                return Ok(dto);
            }

            else
            {
                var dto = _mapper.Map<LooseSectionDetailsDto>(section);
                dto.Presenters = _mapper.Map<PresenterDetailsDto[]>(section.Apresentations);
                return Ok(dto);

            }

        }




        [HttpPost]
        [Route(CreateWeeklyActionRoute)]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateWeeklySection([FromBody] WeeklySectionDto section) => 
            await Create(section);
        
        [HttpPost]
        [Route(CreateLooseActionRoute)]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateLooseSection([FromBody] LooseSectionDto section) => 
            await Create(section);

        [HttpPut]
        [Route(EditWeeklyActionRoute)]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> EditWeeklySection(int id, [FromBody] WeeklySectionDto section) =>
            await Edit(id,section);

        [HttpPut]
        [Route(EditLooseActionRoute)]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> EditLooseSection(int id, [FromBody] LooseSectionDto section) =>
            await Edit(id, section);

        [HttpDelete]
        [Route(Constants.DeleteActionRoute)]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var section = await _context.Sections
                .AsNoTracking()
                .Include(x => x.Apresentations)
                .FirstAsync(x => x.Id == id);

            if (section == null)
            {
                return NotFound();
            }
            
            try
            {
                _context.Remove(section);
                _context.RemoveRange(_context.Apresentations.Where(x => x.SectionId == section.Id));
                await _context.SaveChangesAsync();
                return Ok();

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Erro ao deletar. {ex.Message}");
                ModelState.AddModelError(string.Empty, $"{StringUtils.CannotSaveError}. {ex.Message}");

            }

            return Conflict();
        }

        private async Task<IActionResult> Create<T> (T section) where T : SectionDto
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Section sectionToAdd;
                    if (section is WeeklySectionDto)
                    {
                        sectionToAdd = _mapper.Map<WeeklySection>(section);
                        _context.Add((WeeklySection)sectionToAdd);


                    }
                    else
                    {
                        sectionToAdd = _mapper.Map<LooseSection>(section);
                        _context.Add((LooseSection)sectionToAdd);
                    }

                    //Salvar para obter o id
                    await _context.SaveChangesAsync();

                    //Adicionando apresentações
                    _context.Apresentations.AddRange(Apresentation.CreateObjects(section.PresentersId, sectionToAdd.Id));
                    await _context.SaveChangesAsync();
                    
                    


                    return Ok(section);
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, StringUtils.CannotSaveError);
                _logger.LogError($"Não foi possível adicionar: {ex.Message}", ex);
            }

            return Conflict();
        }

        

        


        private async Task<IActionResult> Edit<T>(int id, T section) where T : SectionDto
        {
            if (ModelState.IsValid)
            {
                try
                {
                    section.Id = id;
                    
                    if (section is WeeklySectionDto)
                    {
                        var sectionToUpdate = _mapper.Map<WeeklySection>(section);
                        _context.Update(sectionToUpdate);
                        

                    } 
                    else
                    {
                        var sectionToUpdate = _mapper.Map<LooseSection>(section);
                        _context.Update(sectionToUpdate);
                    }

                    //Resolvendo apresentações
                    _context.RemoveRange(_context.Apresentations.Where(x => x.SectionId == section.Id));
                    _context.AddRange(Apresentation.CreateObjects(section.PresentersId, id));

                    await _context.SaveChangesAsync();


                    return Ok(section);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Erro de concorrência. ${StringUtils.CannotSaveError}. {ex.Message}");
                    _logger.LogError($"Não foi possível editar: {ex.Message}", ex);
                } catch(DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, StringUtils.CannotSaveError);
                    _logger.LogError($"Não foi possível editar: {ex.Message}", ex);
                }

            }

            return Conflict();
        }

       


       


    }
}
