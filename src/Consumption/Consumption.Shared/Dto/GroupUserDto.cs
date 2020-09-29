using Consumption.Shared.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Consumption.Shared.Dto
{
    public class GroupUserDto : BaseDto
    {
        /// <summary>
        /// 组代码
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        private bool ischecked;

        public bool IsChecked
        {
            get { return ischecked; }
            set { ischecked = value; RaisePropertyChanged(); }
        }
    }
}
