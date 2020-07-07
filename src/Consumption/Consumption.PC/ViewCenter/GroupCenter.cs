using System;
using System.Collections.Generic;
using System.Text;

namespace Consumption.PC.ViewCenter
{
    using Consumption.Core.Attributes;
    using Consumption.PC.View;
    using Consumption.ViewModel;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 用户组
    /// </summary>
    [Module(Core.Enums.ModuleType.DataManagement, "权限管理", "GroupCenter", "", "Group")]
    public class GroupCenter : BaseCenter<GroupView, GroupViewModel>
    {
    }
}
