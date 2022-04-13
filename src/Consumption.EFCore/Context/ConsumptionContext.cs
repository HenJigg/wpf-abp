namespace Consumption.EFCore.Context
{
    using Consumption.Shared.DataModel;
    using Microsoft.EntityFrameworkCore;

    public class ConsumptionContext : DbContext
    {
        public ConsumptionContext(DbContextOptions<ConsumptionContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<GroupFunc> GroupFuncs { get; set; }
        public DbSet<Basic> Basics { get; set; }
        public DbSet<BasicType> BasicTypes { get; set; }
        public DbSet<AuthItem> AuthItems { get; set; }
        public DbSet<UserConfig> UserConfigs { get; set; }
    }
}
