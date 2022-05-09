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
    [ApiController][Route(Constants.ControllerDefaultRoute)]
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

        
        [HttpGet]
        [Route("getAll")]
        public async Task <IActionResult> GetAll(string sortOrder = null)
        {

            var presenters = _context.Presenters;


            switch (sortOrder)
            {
                case "name_desc":
                    return Ok(presenters.OrderByDescending(x => x.Name).ToList());
                default:
                    return Ok(presenters.OrderBy(x => x.Name).ToList());


            }

        }

        // GET: Presenters/Details/5
        [HttpGet("details/")]
        public async Task<IActionResult> Details(int id)
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

            //var presenterDto = PresenterDetailsDto.FromPresenter(presenter, _mapper);
            var presenterDto = _mapper.Map<PresenterDetailsDto>(presenter);
            presenterDto.Sections = _mapper.Map<SectionDto[]>(presenter.Apresentations);

            return Ok(presenterDto);
        }

        
       


        // POST: Presenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("create")]
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
                ModelState.AddModelError(string.Empty, StringUtils.CannotSaveError);
                _logger.LogError($"{StringUtils.CannotSaveError} {ex.Message}", ex);
                
                
            }
            return Conflict();


        }
        

        // POST: Presenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [IgnoreAntiforgeryToken]
        [Route("edit")]
        public async Task<IActionResult> Edit(int id,[FromBody] PresenterDto presenter)
        {
           
            

            if (ModelState.IsValid)
            {
                try
                {
                    presenter.Id = id;
                    var presenterToUpdate = _mapper.Map<PresenterDto, Presenter>(presenter);
                    _context.Update(presenterToUpdate);
                    await _context.SaveChangesAsync();
                    return Ok(presenter);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Erro de concorrência. ${StringUtils.CannotSaveError}. {ex.Message}");
                }
            }

            return Conflict();
        }


        // POST: Presenters/Delete/5
        [IgnoreAntiforgeryToken]
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
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
                ModelState.AddModelError(string.Empty, $"{StringUtils.CannotSaveError}. {ex.Message}");
                
            }

            return Conflict();
        }

    }
}
