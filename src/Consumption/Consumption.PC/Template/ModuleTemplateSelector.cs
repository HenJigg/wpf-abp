using Consumption.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Consumption.PC.Template
{
    public class ModuleTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GroupTemplate { get; set; }
        public DataTemplate ExpanderTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ModuleGroup group = (ModuleGroup)item;
            if (group != null)
            {
                if (!group.ContractionTemplate)
                    return ExpanderTemplate;
                else
                    return GroupTemplate;
            }
            return ExpanderTemplate;
        }
    }
}
