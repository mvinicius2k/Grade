#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grade.Data;
using Grade.Models;
using Grade.Helpers;
using Grade.Models.Dto;
using AutoMapper;

namespace Grade.Controllers
{
    [ApiController][Route("[controller]")]
    public class PresentersController : Controller
    {
        private readonly GradeContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        public PresentersController(GradeContext context, ILogger<PresentersController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: Presenters
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Presenters.ToListAsync());
        }

        // GET: Presenters/Details/5
        [HttpGet("{id}")]
        //[ActionName("details")]
        public async Task<IActionResult> GetOne([FromRoute] int id)
        {
            
            var presenter = await _context.Presenters
                .Include(x => x.Apresentations)
                .ThenInclude(x => x.Section)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (presenter == null)
            {
                return NotFound();
            }

            

            var sections = new SectionDto[presenter.Apresentations.Count];

            var i = 0;
            foreach(var apresentation in presenter.Apresentations)
            {
                if(apresentation.Section is WeeklySection)
                {
                    var weeklySection = (apresentation.Section as WeeklySection);
                    //sections[i++] = new WeeklySectionDto()
                    //{
                    //    Id = weeklySection.Id,
                    //    Active = weeklySection.Active,
                    //    Name = weeklySection.Name,
                    //    Description = weeklySection.Description,
                    //    StartAt = weeklySection.StartAt,
                    //    StartDay = weeklySection.StartDay,
                    //    EndAt = weeklySection.EndAt,
                    //    EndDay = weeklySection.EndDay,
                    //    ImageUrl = weeklySection.Resource?.Url,
                    //};
                    sections[i++] = _mapper.Map<WeeklySectionDto>(weeklySection);
                } else if(apresentation.Section is LooseSection)
                {
                    var looseSection = (apresentation.Section as LooseSection);

                    sections[i++] = new LooseSectionDto()
                    {
                        Id = looseSection.Id,
                        Active = looseSection.Active,
                        Name = looseSection.Name,
                        Description = looseSection.Description,
                        StartAt = looseSection.StartAt,
                        EndAt = looseSection.EndAt,
                        //ImageUrl = looseSection.Resource?.Url,
                        
                    };
                }

                
            }

            var presenterDto = new PresenterDetailsDto()
            {
                Id = presenter.Id,
                //ImageUrl = presenter.Resource?.Url,
                Name = presenter.Name,
                Sections = sections
            };

           
            
            return Ok(presenterDto);
        }

        /*
        // GET: Presenters/Create
        [HttpGet]
        [Route("presenters/create")]
        public IActionResult Create()
        {
            return View();
        }
        */

        // POST: Presenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("presenters/create")]
        public async Task<ActionResult<Presenter>> Create(PresenterDto presenter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var presenterToAdd = _mapper.Map<Presenter>(presenter);
                    _context.Presenters.Add(presenterToAdd);
                    await _context.SaveChangesAsync();
                    return Ok(presenter);
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, StringUtils.DefaultFatalError);
                _logger.LogError($"Não foi possível salvar: {ex.Message}", ex);
                
            }
            return Ok(presenter);
        }

        
       

        // POST: Presenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        //[Route("presenters/edit/")]
        public async Task<IActionResult> Edit([Bind("Id,Name,ResourceId")] Presenter presenter)
        {
            if (_context.Presenters.FirstOrDefault(x => x.Id == presenter.Id) == null)
            {
                return NotFound();
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(presenter);
                    await _context.SaveChangesAsync();
                    return Ok(presenter);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Erro de concorrência. ${StringUtils.DefaultFatalError}. {ex.Message}");
                }
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        /*// GET: Presenters/Delete/5
        [HttpGet]
        [Route("presenters/delete/{id?}")]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presenter = await _context.Presenters
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (presenter == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Erro ao deletar";
            }

            return View(presenter);
        }*/

        // POST: Presenters/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Route("presenters/delete/{id?}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var presenter = await _context.Presenters.FindAsync(id);
            if(presenter == null)
            {
                return NotFound();
            }

            try
            {
                _context.Presenters.Remove(presenter);
                await _context.SaveChangesAsync();
                return Ok();

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Erro ao deletar. {ex.Message}");
                ModelState.AddModelError(string.Empty, $"{StringUtils.DefaultFatalError}. {ex.Message}");
                
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        private bool PresenterExists(int id)
        {
            return _context.Presenters.Any(e => e.Id == id);
        }
    }
}
