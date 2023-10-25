using ExternalTranslator.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExternalTranslator.Controllers;

[ApiController]
[Route("/translate")]
public class TranslationController : ControllerBase
{
    private readonly TranslationService translationService;
    
    public TranslationController(IConfiguration configuration)
    {
        var detectLanguageApiKey = configuration["DetectLanguageClient:ApiKey"];
        translationService = new TranslationService(detectLanguageApiKey);
    }

    [HttpGet]
    [SwaggerResponse(200, "OK")]
    [SwaggerResponse(404, "Impossible to translate")]
    public async Task<IActionResult> Get([FromQuery] string text, [FromQuery] string target, [FromQuery] string? source = null)
    {
        var translation = await translationService.Translate(text, source, target);
        return translation is not null ? Ok(translation) : NotFound();
    }
}