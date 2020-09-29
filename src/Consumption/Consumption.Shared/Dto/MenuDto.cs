using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Consumption.Shared.Dto
{
    public class MenuDto : BaseDto
    {
        /// <summary>
        /// 菜单代码
        /// </summary>
        [Description("菜单代码")]
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string MenuCode { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [Description("菜单名称")]
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单标题
        /// </summary>
        [Description("菜单标题")]
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string MenuCaption { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string MenuNameSpace { get; set; }

        /// <summary>
        /// 所属权限值
        /// </summary>
        public int MenuAuth { get; set; }
    }
}
