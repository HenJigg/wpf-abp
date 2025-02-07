namespace AppFramework.Authorization.Accounts.Dto
{
    public class IsTenantAvailableOutput
    {
        public TenantAvailabilityState State { get; set; }

        public int? TenantId { get; set; }

        public string ServerRootAddress { get; set; }

        public IsTenantAvailableOutput()
        {
            
        }

        public IsTenantAvailableOutput(TenantAvailabilityState state, int? tenantId = null)
        {
            State = state;
            TenantId = tenantId;
        }

        public IsTenantAvailableOutput(TenantAvailabilityState state, int? tenantId, string serverRootAddress)
        {
            State = state;
            TenantId = tenantId;
            ServerRootAddress = serverRootAddress;
        }
    }
}