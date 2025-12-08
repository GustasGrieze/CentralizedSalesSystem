using CentralizedSalesSystem.API.Models;
using CentralizedSalesSystem.API.Models.Auth;
using CentralizedSalesSystem.API.Models.Auth.enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CentralizedSalesSystem.API.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
        {
            await using var scope = services.CreateAsyncScope();
            var scopedProvider = scope.ServiceProvider;
            var context = scopedProvider.GetRequiredService<CentralizedSalesDbContext>();
            var passwordHasher = scopedProvider.GetRequiredService<IPasswordHasher<User>>();

            await context.Database.MigrateAsync(cancellationToken);

            // Seed only when empty to avoid overriding user data.
            if (await context.Users.AnyAsync(cancellationToken))
            {
                return;
            }

            var now = DateTimeOffset.UtcNow;

            var manageAllPermission = new Permission
            {
                Title = "Manage All",
                Description = "Seeded permission for broad access",
                Code = "manage_all",
                Resource = PermissionResource.Business,
                CreatedAt = now,
                UpdatedAt = now,
                Status = Status.Active
            };

            var adminRole = new Role
            {
                Title = "Admin",
                Description = "Seeded administrator role",
                BussinessId = 1,
                CreatedAt = now,
                UpdatedAt = now,
                Status = Status.Active,
                RolePermissions = new List<RolePermission>()
            };

            var adminUser = new User
            {
                Email = "admin@ex.com",
                BusinessId = 1,
                Phone = "+10000000000",
                Status = Status.Active,
                UserRoles = new List<UserRole>()
            };

            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "pass");

            var rolePermission = new RolePermission
            {
                Role = adminRole,
                Permission = manageAllPermission,
                CreatedAt = now,
                UpdatedAt = now
            };

            adminRole.RolePermissions.Add(rolePermission);

            var userRole = new UserRole
            {
                User = adminUser,
                Role = adminRole,
                AssignedAt = now
            };

            adminUser.UserRoles.Add(userRole);

            await context.Permissions.AddAsync(manageAllPermission, cancellationToken);
            await context.Roles.AddAsync(adminRole, cancellationToken);
            await context.Users.AddAsync(adminUser, cancellationToken);
            await context.RolePermissions.AddAsync(rolePermission, cancellationToken);
            await context.UserRoles.AddAsync(userRole, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
