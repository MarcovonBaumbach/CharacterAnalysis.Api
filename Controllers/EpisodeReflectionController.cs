using Microsoft.AspNetCore.Mvc;
using CharacterAnalysis.Api.Application.Analysis;
using CharacterAnalysis.Api.Models;


[ApiController]
[Route("[controller]")]
public class ReflectionController : ControllerBase
{
    private readonly ReflectionService _service;

    public ReflectionController(ReflectionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CharacterAnalysisRequest request)
    {
        var response = await _service.ReflectAsync(
            request.ShowName,
            request.Observations,
            request.Episode);
        return Ok(response);
    }
}
