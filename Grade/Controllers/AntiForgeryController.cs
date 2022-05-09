using Grade.Data;
using Grade.Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace Grade.Controllers
{
    [ApiController][Route( Constants.ControllerDefaultRoute)]
    public class AntiForgeryController : Controller
    {
        public const string AntiforgeryKeyHeader = "X-XRADE-TOKEN";

        private IAntiforgery _antiforgery;
        private readonly GradeContext _context;
        private readonly ILogger _logger;

        public AntiForgeryController(GradeContext context, ILogger<PresentersController> logger, IAntiforgery antiforgery)
        {
            _logger = logger;
            _antiforgery = antiforgery;
        }

        [IgnoreAntiforgeryToken]
        [HttpGet]
        public IActionResult GenerateTokens()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            var requestToken = tokens.RequestToken;
            if (tokens != null && requestToken != null)
            {
                Response.Cookies.Append(AntiforgeryKeyHeader, requestToken, new CookieOptions()
                {
                    HttpOnly = false
                });
                return NoContent();
            }

            return BadRequest("Erro ao obter e gerar o token");

        }
    }
}
