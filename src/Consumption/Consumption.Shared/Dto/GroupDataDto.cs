using Consumption.Shared.DataInterfaces;
using Consumption.Shared.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Consumption.Shared.Dto
{
    public class GroupDataDto : ViewModelBase
    {
        public GroupDataDto()
        {
            group = new Group();
            GroupFuncs = new List<GroupFunc>();
            GroupUsers = new ObservableCollection<GroupUserDto>();
        }
        public Group group { get; set; }

        private ObservableCollection<GroupUserDto> groupUsers;

        /// <summary>
        /// 组所包含用户
        /// </summary>
        public ObservableCollection<GroupUserDto> GroupUsers
        {
            get { return groupUsers; }
            set
            { groupUsers = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 组所包含的模块清单
        /// </summary>
        public List<GroupFunc> GroupFuncs { get; set; }
    }
}
