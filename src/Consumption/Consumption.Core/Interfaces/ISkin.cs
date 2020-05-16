/*
*
* 文件名    ：ISkin                             
* 程序说明  : 基类样式接口
* 更新时间  : 2020-05-11
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/
namespace Consumption.Core.Interfaces
{
    public interface ISkin
    {
        void Apply();

        void ApplyDefault();

        void ApplyBase();
    }
}
