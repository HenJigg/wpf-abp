using System;
using System.Collections.Generic;
using System.Text;

namespace Consumption.PC.ViewCenter
{
    using Consumption.Common.Contract;
    using Consumption.Core.Attributes;
    using Consumption.Core.Entity;
    using Consumption.PC.Common;
    using Consumption.PC.View;
    using Consumption.ViewModel;
    using Consumption.ViewModel.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 用户组
    /// </summary>
    [Module("权限管理", Core.Enums.ModuleType.系统配置)]
    public class GroupCenter : BusinessCenter<GroupView, Group>
    {
        public GroupCenter() : base(NetCoreProvider.Get<IGroupViewModel>())
        { }

        public override void BindDataGridColumns()
        {
            VisualHelper.SetDataGridColumns(view, "Grid", typeof(Group));
        }
    }
}
