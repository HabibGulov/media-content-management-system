using Microsoft.AspNetCore.Mvc;
using FluentValidation;

[Route("/api/users")]
public sealed class UserController : BaseController
{
    private readonly IUserService _userService;
    private readonly IValidator<UserCreateInfo> _userCreateValidator;
    private readonly IValidator<UserUpdateInfo> _userUpdateValidator;

    public UserController(
        IUserService userService,
        IValidator<UserCreateInfo> userCreateValidator,
        IValidator<UserUpdateInfo> userUpdateValidator)
    {
        _userService = userService;
        _userCreateValidator = userCreateValidator;
        _userUpdateValidator = userUpdateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] UserFilter filter)
        => (await _userService.GetUsers(filter)).ToActionResult();

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => (await _userService.GetUserById(id)).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateInfo entity)
    {
        var validationResult = await _userCreateValidator.ValidateAsync(entity);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        return (await _userService.AddUser(entity)).ToActionResult();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateInfo entity)
    {
        var validationResult = await _userUpdateValidator.ValidateAsync(entity);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        return (await _userService.UpdateUser(id, entity)).ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, string password)
    {
        // Возможно, вам стоит добавить дополнительную валидацию для пароля, чтобы убедиться, что он корректен
        return (await _userService.DeleteUser(id, password)).ToActionResult();
    }
}
