public interface IRoleService
{
    Task<Result<bool>> AddRole(RoleCreateInfo roleCreateInfo);
    Task<Result<RoleReadInfo>> GetRoleById(Guid id);
    Task<Result<bool>> UpdateRole(Guid id, RoleUpdateInfo roleUpdateInfo);
    Task<Result<bool>> DeleteRole(Guid id);
    Task<Result<PagedResponse<IEnumerable<RoleReadInfo>>>> GetRoles(RoleFilter filter);
}
