using ANY.Authorization.Tools.Entities;
using Microsoft.EntityFrameworkCore;

namespace ANY.DuendeIDS.Infrastructure.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<RolePermissions> RolePermissions { get; }
}