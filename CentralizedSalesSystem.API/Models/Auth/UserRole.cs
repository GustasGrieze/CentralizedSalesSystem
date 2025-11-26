namespace CentralizedSalesSystem.API.Models.Auth
{
    public class UserRole
    {
        public long Id { get; set; }
        public long UserId{get; set;} 

        public long RoleId{get; set;}
        public DateTimeOffset AssignedAt {get; set;}

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
