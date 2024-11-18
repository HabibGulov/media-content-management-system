using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Threading.Tasks;

[Route("/api/categories")]
public sealed class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    private readonly IValidator<CategoryCreateInfo> _categoryCreateValidator;
    private readonly IValidator<CategoryUpdateInfo> _categoryUpdateValidator;

    public CategoryController(
        ICategoryService categoryService,
        IValidator<CategoryCreateInfo> categoryCreateValidator,
        IValidator<CategoryUpdateInfo> categoryUpdateValidator)
    {
        _categoryService = categoryService;
        _categoryCreateValidator = categoryCreateValidator;
        _categoryUpdateValidator = categoryUpdateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] CategoryFilter filter)
        => (await _categoryService.GetCategories(filter)).ToActionResult();

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(Guid id)
        => (await _categoryService.GetCategoryById(id)).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateInfo entity)
    {
        var validationResult = await _categoryCreateValidator.ValidateAsync(entity);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        return (await _categoryService.AddCategory(entity)).ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateInfo entity)
    {
        var validationResult = await _categoryUpdateValidator.ValidateAsync(entity);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        return (await _categoryService.UpdateCategory(id, entity)).ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(Guid id)
        => (await _categoryService.DeleteCategory(id)).ToActionResult();
}
