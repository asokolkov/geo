using ExternalTranslator.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExternalTranslator.Controllers;

[ApiController]
[Route("/translate")]
public class TranslationController : ControllerBase
{
    private readonly ITranslationService translationService;
    private readonly ILogger<TranslationController> logger;

    public TranslationController(ITranslationService translationService, ILogger<TranslationController> logger)
    {
        this.translationService = translationService;
        this.logger = logger;
        logger.LogInformation("{{msg=\"TranslationController initialized\"}}");
    }

    [HttpGet]
    [SwaggerResponse(200, "OK")]
    [SwaggerResponse(404, "Impossible to translate")]
    public async Task<IActionResult> Get([FromQuery] string text, [FromQuery] string target, [FromQuery] string? source = null)
    {
        var translation = await translationService.Translate(text, source, target);
        if (translation is not null)
        {
            logger.LogInformation("{{msg=\"Sending 200 response with translation\"}}");
        }
        else
        {
            logger.LogInformation("{{msg=\"Sending 404 response\"}}");
        }
        return translation is not null ? Ok(translation) : NotFound();
    }
}