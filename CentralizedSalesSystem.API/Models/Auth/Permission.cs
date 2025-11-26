using CentralizedSalesSystem.API.Models.Auth.enums;

namespace CentralizedSalesSystem.API.Models.Auth
{
    public class Permission
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PermissionResource Resource { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set;}
        public Status Status { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();

    }
}
