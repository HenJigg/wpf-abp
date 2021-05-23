namespace Consumption.Shared.DataInterfaces
{
    using System;

    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILog
    {
        void Error(Exception exception, string message);
        void Error(string message, params object[] args);
        void Error(string message);
        void Info(string message);
        void Info(string message, params object[] args);
        void Info(Exception exception, string message);
        void Warn(string message);
        void Warn(string message, params object[] args);
        void Warn(Exception exception, string message);
        void Debug(string message);
        void Debug(string message, params object[] args);
        void Debug(Exception exception, string message);
    }
}
