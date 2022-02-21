using AutoMapper;
using Grade.Data;
using Grade.Models;
using Grade.Models.Dto;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Grade.Controllers
{
    [ApiController][Route("[controller]")]
    public class SectionsController : Controller
    {
        private readonly GradeContext _context;
        private readonly ILogger _logger;
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
                    .Select(x => _mapper.Map<WeeklySectionDetailsDto>(x))
                    .ToListAsync();

            var looseSections = await _context.LooseSections
                .Where(x => x.Active || includeInactive)
                .Include(x => x.Apresentations)
                .ThenInclude(x => x.Presenter)
                .AsNoTracking()
                .Select(x => _mapper.Map<LooseSectionDetailsDto>(x))
                .ToListAsync();


            var sections = new
            {
                Weekly = new List<WeeklySectionDetailsDto>(weeklySections.Count),
                Loose = new List<LooseSectionDetailsDto>(looseSections.Count)
            };

            switch (sortOrder)
            {
                case "time":
                    sections.Weekly.AddRange(
                        weeklySections
                        .OrderBy(x => x.StartDay)
                        .ThenBy(x => x.StartAt));
                    sections.Loose.AddRange(
                        looseSections.OrderBy(x => x.StartAt));
                    break;

                default:
                    sections.Weekly.AddRange(
                        weeklySections.OrderBy(x => x.Name));
                    sections.Loose.AddRange(
                        looseSections.OrderBy(x => x.Name));
                    break;

            }
            return Ok(sections);

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
                dto.Presenters = _mapper.Map<PresenterDto[]>(section.Apresentations);
                return Ok(dto);
            }

            else
            {
                var dto = _mapper.Map<LooseSectionDetailsDto>(section);
                dto.Presenters = _mapper.Map<PresenterDto[]>(section.Apresentations);
                return Ok(dto);

            }

        }

        

        /*[HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll(string sortOrder = null, bool includeInactive = false)
        {

            
            var weeklySections = _context.WeeklySections.Where(x => x.Active || includeInactive);
            var looseSections = _context.LooseSections.Where(x => x.Active || includeInactive);
           


        
            var sections = new
            {
                Weekly = new List<WeeklySection>(weeklySections.Count()),
                Loose = new List<LooseSection>(looseSections.Count())
            };
            switch (sortOrder)
            {
                case "time":
                    sections.Weekly.AddRange(
                        weeklySections.OrderBy(x => x.StartDay)
                        .ThenBy(x => x.StartAt)
                        .ToList());
                    sections.Loose.AddRange(
                        looseSections.OrderBy(x => x.StartAt)
                        .ToList());
                    break;

                default:
                    sections.Weekly.AddRange(
                        weeklySections.OrderBy(x => x.Name)
                        .ToList());
                    sections.Loose.AddRange(
                        looseSections.OrderBy(x => x.Name)
                        .ToList());
                    break;

            }

            

            return Ok(sections);

        }*/

       
        // POST: SectionController/Create
        [HttpPost]
        [Route("create")]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // POST: SectionController/Edit/5
        [HttpPut]
        [Route("edit")]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
        [HttpDelete]
        [Route("delete")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
