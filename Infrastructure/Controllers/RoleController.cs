using Microsoft.AspNetCore.Mvc;
using FluentValidation;

[Route("/api/roles")]
public sealed class RoleController : BaseController
{
    private readonly IRoleService _roleService;
    private readonly IValidator<RoleCreateInfo> _roleCreateValidator;
    private readonly IValidator<RoleUpdateInfo> _roleUpdateValidator;

    public RoleController(
        IRoleService roleService,
        IValidator<RoleCreateInfo> roleCreateValidator,
        IValidator<RoleUpdateInfo> roleUpdateValidator)
    {
        _roleService = roleService;
        _roleCreateValidator = roleCreateValidator;
        _roleUpdateValidator = roleUpdateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] RoleFilter filter)
        => (await _roleService.GetRoles(filter)).ToActionResult();

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => (await _roleService.GetRoleById(id)).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleCreateInfo entity)
    {
        var validationResult = await _roleCreateValidator.ValidateAsync(entity);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        return (await _roleService.AddRole(entity)).ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RoleUpdateInfo entity)
    {
        var validationResult = await _roleUpdateValidator.ValidateAsync(entity);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        return (await _roleService.UpdateRole(id, entity)).ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => (await _roleService.DeleteRole(id)).ToActionResult();
}
