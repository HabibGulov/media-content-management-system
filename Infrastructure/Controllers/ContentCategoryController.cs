using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Threading.Tasks;

[Route("/api/categorycontent")]
public sealed class CategoryContentController : BaseController
{
    private readonly IContentCategoryService _categoryContentService;
    private readonly IValidator<Category> _categoryValidator;
    private readonly IValidator<ContentCategoryCreateInfo> _categoryContentCreateValidator;
    private readonly IValidator<ContentCategoryCreateInfo> _categoryContentUpdateValidator;

    public CategoryContentController(
        IContentCategoryService categoryContentService,
        IValidator<ContentCategoryCreateInfo> categoryContentCreateValidator,
        IValidator<ContentCategoryCreateInfo> categoryContentUpdateValidator,
        IValidator<Category> categoryValidator)
    {
        _categoryContentService = categoryContentService;
        _categoryContentCreateValidator = categoryContentCreateValidator;
        _categoryContentUpdateValidator = categoryContentUpdateValidator;
        _categoryValidator = categoryValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ContentCategoryFilter filter)
        => (await _categoryContentService.GetContentCategories(filter)).ToActionResult();

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(Guid id)
        => (await _categoryContentService.GetContentCategoryById(id)).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ContentCategoryCreateInfo entity)
    {
        var validationResult = await _categoryContentCreateValidator.ValidateAsync(entity);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        return (await _categoryContentService.AddContentCategory(entity)).ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ContentCategoryUpdateInfo entity)
    {
        // var validationResult = await _categoryContentUpdateValidator.ValidateAsync(entity.FromUpdateToContentCategory());
        // if (!validationResult.IsValid)
        //     return BadRequest(validationResult.Errors);

        return (await _categoryContentService.UpdateContentCategory(id, entity)).ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(Guid id)
        => (await _categoryContentService.DeleteContentCategory(id)).ToActionResult();
}
