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
    [Module("权限管理", Core.Enums.ModuleType.系统配置)]
    public class GroupCenter : BaseCenter<GroupView, GroupViewModel>
    {
    }
}
