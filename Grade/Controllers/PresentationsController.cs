using AutoMapper;
using Grade.Data;
using Grade.Helpers;
using Grade.Models;
using Grade.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grade.Controllers
{
    [ApiController][Route(Constants.ControllerDefaultRoute)]
    public class PresentationsController : Controller
    {
        private const string GetByPresenterActionName = $"{Constants.GetActionRoute}ByPresenter";
        private const string GetBySectionActionName = $"{Constants.GetActionRoute}BySection";

        private readonly GradeContext _context;
        private readonly ILogger _logger;

        public PresentationsController(GradeContext context, ILogger<PresentationsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
        }

        
        [HttpGet, Route(GetByPresenterActionName)]
        public async Task<IActionResult> GetByPresenterId(int presenterId, bool includeInactive = false)
        {
            
            if (ModelState.IsValid)
            {
                var presentations = await _context.Presentations
                    .Where(a => (includeInactive || a.Section.Active) && a.PresenterId == presenterId)
                    .ToArrayAsync();
                return Ok(presentations);
            }
            return Conflict();

        }

        [HttpGet, Route(GetBySectionActionName)]
        public async Task<IActionResult> GetBySection(int sectionId, bool includeInactive = false)
        {

            if (ModelState.IsValid)
            {
                var presentations = await _context.Presentations
                    .Where(x => (includeInactive || x.Section.Active) && x.SectionId == sectionId)
                    .ToArrayAsync();
                return Ok(presentations);
            }
            return Conflict();

        }


        [HttpPost, Route(Constants.CreateActionRoute)]
        public async Task<IActionResult> Create([FromBody]ICollection<Presentation> presentations)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    _context.AddRange(presentations);
                    await _context.SaveChangesAsync();
                    return Ok(presentations);
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, StringUtils.CannotSaveError);
                _logger.LogError($"{StringUtils.CannotSaveError} {ex.Message}", ex);
            }

            return Conflict();
        }
        [HttpPut, Route(Constants.EditActionRoute),]
        public async Task<IActionResult> Edit([FromBody] ICollection<Presentation> presentations)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.UpdateRange(presentations);
                    await _context.SaveChangesAsync();
                    return Ok(presentations);

                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, StringUtils.CannotSaveError);
                    _logger.LogError($"{StringUtils.CannotSaveError} {ex.Message}", ex);
                }
            }

            return Conflict();
        }
        [HttpDelete, Route(Constants.DeleteActionRoute),]
        public async Task<IActionResult> Delete(ICollection<int> ids)
        {
            var presentations = await _context.Presentations.Where(x => ids.Contains(x.Id)).ToListAsync();
            if (presentations == null || presentations.Count == 0)
                return NotFound();

            try
            {
                _context.RemoveRange(presentations);
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
