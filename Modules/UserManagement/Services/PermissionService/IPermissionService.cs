public interface IPermissionService
{
    Task<Result<bool>> AddPermission(PermissionCreateInfo permissionCreateInfo);
    Task<Result<PermissionReadInfo>> GetPermissionById(Guid id);
    Task<Result<PagedResponse<IEnumerable<PermissionReadInfo>>>> GetPermissions(PermissionFilter filter);
    Task<Result<bool>> UpdatePermission(Guid id, PermissionUpdateInfo permissionUpdateInfo);
    Task<Result<bool>> DeletePermission(Guid id);
}