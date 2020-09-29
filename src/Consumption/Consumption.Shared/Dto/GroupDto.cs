

namespace Consumption.Shared.Dto
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class GroupDto : BaseDto
    {
        /// <summary>
        /// 组代码
        /// </summary>
        [Description("组代码")]
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string GroupCode { get; set; }

        /// <summary>
        /// 组名称
        /// </summary>
        [Description("组名称")]
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string GroupName { get; set; }
    }
}
