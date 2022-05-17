using AutoMapper;
using Grade.Data;
using Grade.Helpers;
using Grade.Models;
using Grade.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grade.Controllers;
[ApiController, Route(Constants.ControllerDefaultRoute)]
public class ResourcesController : Controller
{

    #region Constants

    #endregion

    private readonly GradeContext _context;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public static readonly string PathDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

    public ResourcesController(GradeContext context, ILogger<ResourcesController> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("getAll")]
    public ActionResult GetAll()
    {
        return Ok(_context.Resources
            .OrderBy(x => x.UploadedAt)
            .Select(x => _mapper.Map<Resource, ResourceDetailsDto>(x)));
    }

    [HttpPost]
    [Route("uploadImage")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> UploadImage(List<IFormFile> files)
    {
        try
        {
            if (files.Count == 0)
                return BadRequest();

            foreach (var file in files)
            {
                if (string.IsNullOrWhiteSpace(file.FileName))
                {
                    _logger.LogDebug($"{file.ToString()} inválido");
                    continue;
                }

                var filename = file.FileName.Trim();

                var finalPath = Path.Combine(PathDirectory, filename);

                if (System.IO.File.Exists(finalPath))
                {
                    _logger.LogDebug($"{file.ToString()} já existe com o mesmo nome \"{filename}\"");
                    continue;
                }
                    

                using (var stream = new FileStream(finalPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { finalPath });
            }


        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return StatusCode(500);
        }

        return BadRequest();
    }


    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("link")]
    public async Task<IActionResult> Link([FromBody] ResourceDto resource)
    {

        var resourceToAdd = _mapper.Map<ResourceDto, Resource>(resource);
        resourceToAdd.UploadedAt = DateTime.Now;
        resourceToAdd.ResolveTypesPgsql();
        if (ModelState.IsValid)
        {
            _context.Add(resourceToAdd);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<Resource, ResourceDetailsDto>(resourceToAdd));
        }

        return Conflict();

    }
    [HttpPut]
    [IgnoreAntiforgeryToken]
    [Route("replace")]
    public async Task<IActionResult> Replace(int id, [FromBody] ResourceDto resource)
    {
        if (ModelState.IsValid)
        {
            resource.Id = id;
            var resourceToReplace = _mapper.Map<ResourceDto, Resource>(resource);
            _context.Update(resourceToReplace);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<Resource, ResourceDetailsDto>(resourceToReplace));
        }

        return Conflict();
    }

    [HttpDelete]
    [IgnoreAntiforgeryToken]
    [Route("delete")]
    public async Task<IActionResult> Delete(int id, bool preventDeleteAUsefull = true)
    {
        Resource? resource = null;

        if (preventDeleteAUsefull)
        {
            resource = await _context.Resources
                .Include(x => x.Presenters)
                .Include(x => x.Sections)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        }
        else
        {
            resource = await _context.Resources.FirstOrDefaultAsync(x => x.Id == id);
        }

        if (resource == null)
            return NotFound();

        if(preventDeleteAUsefull && (resource.Sections.Any() || resource.Sections.Any()))
            return Ok(StatusCodes.Status304NotModified);

        _context.Resources.Remove(resource);
        return Ok();
    }

    

}
