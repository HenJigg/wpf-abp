using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Consumption.Shared.DataModel
{
    public class MenuModuleGroup
    {
        public string MenuCode { get; set; }

        public string MenuName { get; set; }

        public ObservableCollection<MenuModule> Modules { get; set; }
    }

    public class MenuModule
    {
        public string Name { get; set; }

        public int Value { get; set; }
    }
}
