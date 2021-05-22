namespace Consumption.EFCore
{
    using Consumption.EFCore.Context;
    using Consumption.Shared.DataModel;

    public class CustomUserRepository : Repository<User>, IRepository<User>
    {
        public CustomUserRepository(ConsumptionContext dbContext) : base(dbContext) { }
    }

    public class CustomUserLogRepository : Repository<UserLog>, IRepository<UserLog>
    {
        public CustomUserLogRepository(ConsumptionContext dbContext) : base(dbContext) { }
    }

    public class CustomMenuRepository : Repository<Menu>, IRepository<Menu>
    {
        public CustomMenuRepository(ConsumptionContext dbContext) : base(dbContext) { }
    }

    public class CustomGroupRepository : Repository<Group>, IRepository<Group>
    {
        public CustomGroupRepository(ConsumptionContext dbContext) : base(dbContext) { }
    }

    public class CustomBasicRepository : Repository<Basic>, IRepository<Basic>
    {
        public CustomBasicRepository(ConsumptionContext dbContext) : base(dbContext) { }
    }

    public class CustomAuthItemRepository : Repository<AuthItem>, IRepository<AuthItem>
    {
        public CustomAuthItemRepository(ConsumptionContext dbContext) : base(dbContext) { }
    }
}
