using Abp;

namespace AppFramework.Editions
{
    public class MoveTenantsToAnotherEditionJobArgs
    {
        /// <summary>
        /// Id of the source edition to move tenants from
        /// </summary>
        public int SourceEditionId { get; set; }

        /// <summary>
        /// Id of the target edition for tenants to move
        /// </summary>
        public int TargetEditionId { get; set; }

        /// <summary>
        /// Identifier of the user who starts move operation
        /// </summary>
        public UserIdentifier User { get; set; }
    }
}