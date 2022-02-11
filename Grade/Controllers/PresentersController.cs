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
        
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Presenters.ToListAsync());
        }

        // GET: Presenters/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
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

            var presenterDto = _mapper.Map<Presenter, PresenterDetailsDto>(presenter);

            presenterDto.Sections = new SectionDto[presenter.Apresentations.Count];
            var i = 0;
            foreach (var apresentation in presenter.Apresentations)
            {
                if(apresentation.Section is WeeklySection)
                    presenterDto.Sections[i++] = _mapper.Map<WeeklySection, WeeklySectionDetailsDto>(apresentation.Section as WeeklySection);
                else if (apresentation.Section is LooseSection)
                    presenterDto.Sections[i++] = _mapper.Map<LooseSection, LooseSectionDetailsDto>(apresentation.Section as LooseSection);
            }
            
            
            return Ok(presenterDto);
        }


        // POST: Presenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Create([FromBody] PresenterDto presenter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var presenterToAdd = _mapper.Map<PresenterDto, Presenter>(presenter);
                    


                    _context.Add(presenterToAdd);
                    await _context.SaveChangesAsync();
                    return Ok(presenter);
                }

            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, StringUtils.DefaultFatalError);
                _logger.LogError($"Não foi possível salvar: {ex.Message}", ex);
                
                
            }
            return Conflict();


        }
        

        // POST: Presenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Presenter presenter)
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


        // POST: Presenters/Delete/5
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
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
