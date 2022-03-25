using AutoMapper;
using Grade.Data;
using Grade.Helpers;
using Grade.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grade.Controllers
{
    internal class ApresentationsController : Controller
    {
        private const string GetByPresenter = $"{Constants.GetActionRoute}ByPresenter";
        private const string GetBySection = $"{Constants.GetActionRoute}BySection";

        private readonly GradeContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        internal ApresentationsController(GradeContext context, ILogger<ApresentationsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet, Route(GetByPresenter)]
        internal async Task<ActionResult<ICollection<Apresentation>>> GetByPresenterId(int presenterId, bool includeInactive = false)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, StringUtils.CannotSaveError);
                _logger.LogError($"{StringUtils.CannotQueryError} {ex.Message}", ex);
            }

            return Conflict();
        }

        [HttpGet, Route(Constants.CreateActionRoute)]
        internal async Task<IActionResult> Create([FromBody]ICollection<Apresentation> apresentations)
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


    }
}
