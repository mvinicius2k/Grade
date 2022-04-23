using AutoMapper;
using Grade.Data;
using Grade.Helpers;
using Grade.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grade.Controllers
{
    public class ApresentationsController : Controller
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

        [HttpGet, Route(Constants.CreateActionRoute)]
        public async Task<IActionResult> Create([FromBody]ICollection<Apresentation> apresentations)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(await InternalCreate(apresentations));
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, StringUtils.CannotSaveError);
                _logger.LogError($"{StringUtils.CannotSaveError} {ex.Message}", ex);
            }

            return Conflict();
        }
        
        internal async Task<ICollection<Apresentation>> InternalCreate(ICollection<Apresentation> apresentations)
        {
            _context.AddRange(apresentations);
            await _context.SaveChangesAsync();
            return apresentations;
        }


    }
}
