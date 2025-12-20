using Microsoft.AspNetCore.Mvc;
using CharacterAnalysis.Api.Application.Analysis;

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
    public async Task<IActionResult> Post([FromBody] string prompt)
    {
        var response = await _service.ReflectAsync(prompt);
        return Ok(response);
    }
}
