using Consumption.Shared.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Consumption.Shared.Dto
{
    public class UserInfoDto
    {
        public User User { get; set; }

        public List<Menu> Menus { get; set; }
    }
}
