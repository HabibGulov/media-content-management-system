using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Threading.Tasks;

[Route("/api/permissions")]
public sealed class PermissionController : BaseController
{
    private readonly IPermissionService _permissionService;
    private readonly IValidator<PermissionCreateInfo> _permissionCreateValidator;
    private readonly IValidator<PermissionUpdateInfo> _permissionUpdateValidator;

    public PermissionController(IPermissionService permissionService,
        IValidator<PermissionCreateInfo> permissionCreateValidator,
        IValidator<PermissionUpdateInfo> permissionUpdateValidator)
    {
        _permissionService = permissionService;
        _permissionCreateValidator = permissionCreateValidator;
        _permissionUpdateValidator = permissionUpdateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PermissionFilter filter)
        => (await _permissionService.GetPermissions(filter)).ToActionResult();

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(Guid id)
        => (await _permissionService.GetPermissionById(id)).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PermissionCreateInfo entity)
    {
        var validationResult = await _permissionCreateValidator.ValidateAsync(entity);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToDictionary());

        return (await _permissionService.AddPermission(entity)).ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PermissionUpdateInfo entity)
    {
        var validationResult = await _permissionUpdateValidator.ValidateAsync(entity);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToDictionary());

        return (await _permissionService.UpdatePermission(id, entity)).ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(Guid id)
        => (await _permissionService.DeletePermission(id)).ToActionResult();
}
