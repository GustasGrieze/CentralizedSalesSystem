using CentralizedSalesSystem.API.Models.Auth;
using CentralizedSalesSystem.API.Models.Auth.enums;

namespace CentralizedSalesSystem.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }

        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public Status Status{  get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    }
}
