using Prism.Commands;
using Prism.Mvvm; 
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace AppFramework.Admin.MaterialUI.Themes.Controls
{
    internal class DataPager : Control, INotifyPropertyChanged
    {
        public DataPager()
        {
            ButtonCollections = new ObservableCollection<ButtonCommandModel>();
        }

        private ListBox _listBoxButtonList;
        private ComboBox comboBoxPageSize;
        private int SelectedIndex;

        /// <summary>
        /// 显示按钮数量
        /// </summary>
        public int NumericButtonCount
        {
            get { return (int)GetValue(NumericButtonCountProperty); }
            set { SetValue(NumericButtonCountProperty, value); }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        internal ObservableCollection<ButtonCommandModel> ButtonCollections
        {
            get; set;
        }

        public static readonly DependencyProperty NumericButtonCountProperty =
            DependencyProperty.Register("NumericButtonCount", typeof(int), typeof(DataPager), new PropertyMetadata(NumericButtonCountChangedCallback));

        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(DataPager), new PropertyMetadata(PageCountChangedCallback));

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(DataPager), new PropertyMetadata(PageSizeChangedCallback));

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(DataPager), new PropertyMetadata(PageIndexChangedCallback));

        public event PropertyChangedEventHandler? PropertyChanged;

        public override void OnApplyTemplate()
        {
            var listBox = GetTemplateChild("ItemsControl") as ListBox;

            if (listBox != null)
            {
                _listBoxButtonList=listBox;
                _listBoxButtonList.ItemsSource = ButtonCollections;
            }

            if (GetTemplateChild("COMBOX_PAGESIZE") is ComboBox comboBoxPageSize)
            {
                this.comboBoxPageSize = comboBoxPageSize;
                this.comboBoxPageSize.SelectedIndex = SelectedIndex;
                this.comboBoxPageSize.SelectionChanged += ComboBoxPageSize_SelectionChanged;
            }

            GetTemplateButtonByName("HomePage").Click += HomePage_Click;
            GetTemplateButtonByName("PreviousPage").Click += PreviousPage_Click;
            GetTemplateButtonByName("NextPage").Click += NextPage_Click;
            GetTemplateButtonByName("EndPage").Click += EndPage;

            base.OnApplyTemplate();
        }

        void ButtonIndexClick(ButtonCommandModel arg)
        {
            PageIndex = arg.Index-1; //设置当前页索引    
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HomePage_Click(object sender, RoutedEventArgs e)
        {
            PageIndex=0;
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex > 0) PageIndex-=1;
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex+1==PageCount) return;

            if (PageIndex < PageCount) PageIndex+=1;
        }

        /// <summary>
        /// 尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EndPage(object sender, RoutedEventArgs e)
        {
            if (PageIndex != PageCount) PageIndex = PageCount-1;  //设置尾页 
        }

        private Button GetTemplateButtonByName(string Name)
        {
            return GetTemplateChild(Name) as Button;
        }

        private static void NumericButtonCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataPager = (DataPager)d;

            dataPager.ButtonCollections.Clear();

            if (dataPager.PageCount == 0)
            {
                dataPager.ButtonCollections.Add(new ButtonCommandModel
                {
                    Index=1,
                    ClickCommand=new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                });
                return;
            }

            if (int.TryParse(e.NewValue.ToString(), out int buttonCount))
            {
                for (int i = 1; i < buttonCount + 1; i++)
                {
                    dataPager.ButtonCollections.Add(new ButtonCommandModel
                    {
                        Index=i,
                        ClickCommand=new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                    });
                }
            }
        }

        private static void PageSizeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataPager = (DataPager)d;

            if (int.TryParse(e.NewValue.ToString(), out int result))
            {
                switch (result)
                {
                    case 10: dataPager.SelectedIndex = 0; break;
                    case 20: dataPager.SelectedIndex = 1; break;
                    case 50: dataPager.SelectedIndex = 2; break;
                    case 100: dataPager.SelectedIndex = 3; break;
                    default: dataPager.SelectedIndex = 1; break;
                }
            }
        }

        private static void PageCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataPager = (DataPager)d;

            dataPager.ButtonCollections.Clear();

            if (dataPager.PageCount == 0)
            {
                dataPager.ButtonCollections.Add(new ButtonCommandModel
                {
                    Index=1,
                    ClickCommand=new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                });
                return;
            }

            if (int.TryParse(e.NewValue.ToString(), out int pageCount))
            {
                int Count = 0;
                if (dataPager.NumericButtonCount > pageCount)
                    Count = pageCount;
                else
                    Count = dataPager.NumericButtonCount;

                for (int i = 1; i < Count + 1; i++)
                {
                    dataPager.ButtonCollections.Add(new ButtonCommandModel
                    {
                        Index=i,
                        ClickCommand=new DelegateCommand<ButtonCommandModel>(dataPager.ButtonIndexClick)
                    });
                }
            }
        }

        private static void PageIndexChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        { 
            var dataPager = (DataPager)d;
            var newValue = (int)e.NewValue+1;

            if (newValue==1)
            {
                dataPager.RefreshButtonList(newValue); 
            }
            else
            {
                var item = dataPager.ButtonCollections.FirstOrDefault(t => t.Index.Equals(newValue));
                if (item==null)
                    dataPager.RefreshButtonList(newValue-dataPager.NumericButtonCount+1); 
            }
            var selectedItem = dataPager.ButtonCollections.FirstOrDefault(t => t.Index.Equals(newValue));
            if (selectedItem!=null)
                dataPager._listBoxButtonList.SelectedItem=selectedItem;
        }

        private void ComboBoxPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (comboBoxPageSize.SelectedIndex)
            {
                case 0: PageSize = 10; break;
                case 1: PageSize = 20; break;
                case 2: PageSize = 50; break;
                case 3: PageSize = 100; break;
                default: PageSize = 10; break;
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RefreshButtonList(int Index)
        {
            for (int i = 0; i < ButtonCollections.Count; i++)
            {
                ButtonCollections[i].Index=Index;
                Index++;
            }
        }

        internal class ButtonCommandModel : BindableBase
        {
            private int index;
            public int Index
            {
                get { return index; }
                set { index=value; RaisePropertyChanged(); }
            }

            public DelegateCommand<ButtonCommandModel> ClickCommand { get; set; }
        }
    }
}
