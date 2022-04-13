using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Consumption.Shared.Dto
{
    public class BasicDto : BaseDto
    {
        /// <summary>
        /// 字典类型代码
        /// </summary>
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string TypeCode { get; set; }

        /// <summary>
        /// 数据编号
        /// </summary>
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string DataCode { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string NativeName { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public string LastUpdateBy { get; set; }
    }
}
