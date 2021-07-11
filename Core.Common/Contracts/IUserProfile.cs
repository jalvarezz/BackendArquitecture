
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface IUserProfile
    {
        Guid UserId { get; }

        Guid RoleId { get; }

        string UserName { get; }

        string FullName { get; }

        string RoleName { get; }

        string Email { get; }

        Task<bool> HasPermissionAsync(string permissionName);
    }
}
