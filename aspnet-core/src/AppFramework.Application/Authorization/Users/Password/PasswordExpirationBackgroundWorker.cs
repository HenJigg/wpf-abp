using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;

namespace AppFramework.Authorization.Users.Password
{
    public class PasswordExpirationBackgroundWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private const int CheckPeriodAsMilliseconds = 1 * 60 * 60 * 1000 * 24; //1 day

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IPasswordExpirationService _passwordExpirationService;

        public PasswordExpirationBackgroundWorker(
            AbpTimer timer,
            IUnitOfWorkManager unitOfWorkManager,
            IPasswordExpirationService passwordExpirationService)
            : base(timer)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _passwordExpirationService = passwordExpirationService;

            Timer.Period = CheckPeriodAsMilliseconds;
            Timer.RunOnStart = true;

            LocalizationSourceName = AppFrameworkConsts.LocalizationSourceName;
        }

        protected override void DoWork()
        {
            _unitOfWorkManager.WithUnitOfWork(() =>
            {
                _passwordExpirationService.ForcePasswordExpiredUsersToChangeTheirPassword();
            });
        }
    }

}
