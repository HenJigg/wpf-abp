using GalaSoft.MvvmLight;
using NoteMoblie.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoteMoblie.Core.BusinessTemp
{
    /// <summary>
    /// 用户帐单模型
    /// </summary>
    public class UserBillEntity : ViewModelBase
    {
        /// <summary>
        /// 消费种类
        /// </summary>
        public ConsumptionType Type { get; set; }

        /// <summary>
        /// 消费描述
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
    }
}
