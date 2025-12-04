using CentralizedSalesSystem.API.Models;
using CentralizedSalesSystem.API.Models.Auth;
using CentralizedSalesSystem.API.Models.Orders;

using Microsoft.EntityFrameworkCore;

namespace CentralizedSalesSystem.API.Data
{
    public class CentralizedSalesDbContext: DbContext
    {   
        //Auth
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; } 
        public DbSet<RolePermission> RolePermissions { get; set; }

        //Orders
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ServiceCharge> ServiceCharges { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Tax> Taxes { get; set; }

        // Reservations
        public DbSet<Reservation> Reservations { get; set; }






        public CentralizedSalesDbContext(DbContextOptions<CentralizedSalesDbContext> options) : base(options) { 
                
        }
    }
}
