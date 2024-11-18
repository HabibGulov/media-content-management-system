using Microsoft.AspNetCore.Mvc;
using FluentValidation;

[Route("/api/content")]
public sealed class ContentController : BaseController
{
    private readonly IContentService _contentService;
    private readonly IValidator<ContentCreateInfo> _contentCreateValidator;
    private readonly IValidator<ContentUpdateInfo> _contentUpdateValidator;

    public ContentController(
        IContentService contentService,
        IValidator<ContentCreateInfo> contentCreateValidator,
        IValidator<ContentUpdateInfo> contentUpdateValidator)
    {
        _contentService = contentService;
        _contentCreateValidator = contentCreateValidator;
        _contentUpdateValidator = contentUpdateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ContentFilter filter)
        => (await _contentService.GetAllContents(filter)).ToActionResult();

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => (await _contentService.GetContentById(id)).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContentCreateInfo entity, Guid userId)
    {
        var validationResult = await _contentCreateValidator.ValidateAsync(entity);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        return (await _contentService.CreateContent(entity, userId)).ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ContentUpdateInfo entity, Guid userId)
    {
        var validationResult = await _contentUpdateValidator.ValidateAsync(entity);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        return (await _contentService.UpdateContent(id, entity, userId)).ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, Guid userId)
        => (await _contentService.DeleteContent(id, userId)).ToActionResult();
}
