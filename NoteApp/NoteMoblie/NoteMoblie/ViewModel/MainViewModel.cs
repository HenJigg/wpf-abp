using GalaSoft.MvvmLight;
using NoteMoblie.Core.TempClass;
using NoteMoblie.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace NoteMoblie.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Modules = new ObservableCollection<MainTabbedPageEntity>();
            Modules.Add(new MainTabbedPageEntity() { Title = "记账", Icon = "\xe64a", Content = new UserView() });
            Modules.Add(new MainTabbedPageEntity() { Title = "明细", Icon = "\xe664" });
            Modules.Add(new MainTabbedPageEntity() { Title = "图表", Icon = "\xe615" });
            Modules.Add(new MainTabbedPageEntity() { Title = "我的", Icon = "\xe617" });
        }

        public ObservableCollection<MainTabbedPageEntity> Modules { get; set; }

    }
}
