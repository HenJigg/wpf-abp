using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace AppFramework.Shared
{
    /// <summary>
    /// 字符串多语言转换扩展
    /// </summary>
    public class TranslateExtension : MarkupExtension
    {
        public TranslateExtension(string text)
        {
            Text = text;
        }

        /// <summary>
        /// 本地化字符串
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 返回当前语言的对应文本
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target
                && target.TargetObject is FrameworkElement element
                && DesignerProperties.GetIsInDesignMode(element))
            {
                return Text;
            }

            if (string.IsNullOrWhiteSpace(Text)) return Text;

            return Local.Localize(Text);
        }
    }
}