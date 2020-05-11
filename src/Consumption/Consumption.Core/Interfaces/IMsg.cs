/*
*
* 文件名    ：IMsg                             
* 程序说明  : 消息提示接口
* 更新时间  : 2020-05-11
* 
* 
*
*/

namespace Consumption.Core.Interfaces
{
    /// <summary>
    /// 消息接口
    /// </summary>
    public interface IMsg
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        /// <param name="msg">内容</param>
        void Error(string msg);

        /// <summary>
        /// 消息提示
        /// </summary>
        /// <param name="msg">内容</param>
        void Info(string msg);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg">内容</param>
        void Warning(string msg);

        /// <summary>
        /// 询问
        /// </summary>
        /// <param name="msg">内容</param>
        void Question(string msg);

        /// <summary>
        /// 弹出
        /// </summary>
        void Show();
    }
}
