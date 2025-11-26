namespace CentralizedSalesSystem.API.Models.Auth
{
    public class RolePermission
    {
        public long Id { get; set; }
        public long RoleID { get; set; }

        public long PermissionID { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get;set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }

    }
}
