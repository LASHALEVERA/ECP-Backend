using ECPAPI.Data.Configs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ECPAPI.Data
{
    public class ECPDbContext : DbContext
    {
        public ECPDbContext(DbContextOptions<ECPDbContext> options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new UserTypeConfig());
            modelBuilder.ApplyConfiguration(new UserRoleMappingConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new RolePrivilegeConfig());

            modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 2);
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<OrderItem>().Property(o => o.Price).HasPrecision(18, 2);
            //base.OnModelCreating(modelBuilder);
            //    modelBuilder.Entity<Product>().Property(x => x.Images).HasConversion(
            //v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            //v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null))
            //.HasColumnType("nvarchar(max)");
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePrivilege> RolePrivileges { get; set; }
        public DbSet<UserRoleMapping> UserMappings { get; set; }
        public DbSet<UserType> UserTypes { get; set; }

    }
}
