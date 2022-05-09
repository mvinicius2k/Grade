using AutoMapper;
using Grade.Data;
using Grade.Exceptions;
using Grade.Helpers;
using Grade.Models;
using Grade.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grade.Controllers
{
    [ApiController][Route(Constants.ControllerDefaultRoute)]
    public class ApresentationsController : Controller
    {
        private const string GetByPresenterActionName = $"{Constants.GetActionRoute}ByPresenter";
        private const string GetBySectionActionName = $"{Constants.GetActionRoute}BySection";

        private readonly GradeContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        internal ApresentationsController(GradeContext context, ILogger<ApresentationsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        
        [HttpGet, Route(GetByPresenterActionName)]
        public async Task<IActionResult> GetByPresenterId(int presenterId, bool includeInactive = false)
        {
            
            if (ModelState.IsValid)
            {
                var apresentations = await _context.Apresentations
                    .Where(a => (includeInactive || a.Section.Active) && a.PresenterId == presenterId)
                    .ToArrayAsync();
                return Ok(apresentations);
            }
            return Conflict();

        }

        [HttpGet, Route(GetBySectionActionName)]
        public async Task<IActionResult> GetBySection(int sectionId, bool includeInactive = false)
        {

            if (ModelState.IsValid)
            {
                var apresentations = await _context.Apresentations
                    .Where(x => (includeInactive || x.Section.Active) && x.SectionId == sectionId)
                    .ToArrayAsync();
                return Ok(apresentations);
            }
            return Conflict();

        }


        [HttpPost, Route(Constants.CreateActionRoute), IgnoreAntiforgeryToken]
        public async Task<IActionResult> Create([FromBody]ICollection<Apresentation> apresentations)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    _context.AddRange(apresentations);
                    await _context.SaveChangesAsync();
                    return Ok(apresentations);
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, StringUtils.CannotSaveError);
                _logger.LogError($"{StringUtils.CannotSaveError} {ex.Message}", ex);
            }

            return Conflict();
        }
        [HttpPut, Route(Constants.EditActionRoute), IgnoreAntiforgeryToken]
        public async Task<IActionResult> Edit([FromBody] ICollection<Apresentation> apresentations)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.UpdateRange(apresentations);
                    await _context.SaveChangesAsync();
                    return Ok(apresentations);

                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, StringUtils.CannotSaveError);
                    _logger.LogError($"{StringUtils.CannotSaveError} {ex.Message}", ex);
                }
            }

            return Conflict();
        }
        [HttpDelete, Route(Constants.DeleteActionRoute), IgnoreAntiforgeryToken]
        public async Task<IActionResult> Delete(ICollection<int> ids)
        {
            var apresentations = await _context.Apresentations.Where(x => ids.Contains(x.Id)).ToListAsync();
            if (apresentations == null || apresentations.Count == 0)
                return NotFound();

            try
            {
                _context.RemoveRange(apresentations);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException ex)
            {

                ModelState.AddModelError(string.Empty, StringUtils.CannotSaveError);
                _logger.LogError($"{StringUtils.CannotDeleteError} {ex.Message}", ex);
            }

            return Conflict();
        }


    }
}
