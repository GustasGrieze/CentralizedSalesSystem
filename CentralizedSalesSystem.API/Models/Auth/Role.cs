using CentralizedSalesSystem.API.Models.Auth.enums;
using System.Reflection;

namespace CentralizedSalesSystem.API.Models.Auth
{
    public class Role
    {
        public int Id { get; set; }
        public int BussinessId {  get; set; }

        public String Title { get; set; } = string.Empty;
        public String? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public Status Status { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
