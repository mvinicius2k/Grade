using Grade.Data;
using Grade.Helpers;
using Grade.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grade.Controllers
{
    [ApiController][Route("[controller]")]
    public class ApresentationsController : Controller
    {
        private readonly GradeContext _context;
        private readonly ILogger _logger;

        public ApresentationsController(GradeContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        [NonAction]
        public async Task<IActionResult> AddAll(Apresentation[] apresentations )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Apresentations.AddRange(apresentations);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (DbUpdateException ex)
            {

                ModelState.AddModelError(string.Empty, StringUtils.DefaultFatalError);
                _logger.LogError($"Não foi possível salvar: {ex.Message}", ex);
            }

            return Conflict();
            


        }
    }
}
