
using System;
using System.Collections.Generic;

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

        bool HasPermission(string permissionName);
    }
}
