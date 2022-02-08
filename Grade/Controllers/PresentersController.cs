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

namespace Grade.Controllers
{

    public class PresentersController : Controller
    {
        private readonly GradeContext _context;
        private readonly ILogger _logger;


        public PresentersController(GradeContext context, ILogger<PresentersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Presenters
        [HttpGet]
        [Route("presenters")]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Presenters.ToListAsync());
        }

        // GET: Presenters/Details/5
        [HttpGet]
        [Route("presenters/details/")]
        public async Task<IActionResult> Details([FromQuery] int? id)
        {
            if (id == null)
            {
                
                return NotFound();
            }

            var presenter = await _context.Presenters
                .Include(x => x.Apresentations)
                .ThenInclude(x => x.Section)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (presenter == null)
            {
                return NotFound();
            }
            
            return Ok(presenter);
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
        [Route("presenters/create")]
        public async Task<IActionResult> Create([Bind("Name,ResourceId")] Presenter presenter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(presenter);
                    await _context.SaveChangesAsync();
                    return Ok(presenter);
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(String.Empty, StringUtils.DefaultFatalError);
                _logger.LogError($"Não foi possível salvar: {ex.Message}", ex);
                
            }
            return Ok(presenter);
        }

        // GET: Presenters/Edit/5
        /*[HttpGet]
        [Route("presenters/edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presenter = await _context.Presenters.FindAsync(id);
            if (presenter == null)
            {
                return NotFound();
            }
            return View(presenter);
        }*/

        // POST: Presenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("presenters/edit/{id?}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ResourceId")] Presenter presenter)
        {
            if (id != presenter.Id)
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
        [Route("presenters/delete/{id?}")]
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
