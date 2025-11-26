using CentralizedSalesSystem.API.Models;
using CentralizedSalesSystem.API.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace CentralizedSalesSystem.API.Data
{
    public class CentralizedSalesDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; } 
        public DbSet<RolePermission> RolePermissions { get; set; }

        public CentralizedSalesDbContext(DbContextOptions<CentralizedSalesDbContext> options) : base(options) { 
                
        }
    }
}
