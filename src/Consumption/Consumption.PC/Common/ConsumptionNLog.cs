namespace Consumption.Core.Common
{
    using Consumption.Shared.DataInterfaces;
    using NLog;
    using System;

    /// <summary>
    /// Nlog
    /// </summary>
    public class ConsumptionNLog : ILog
    {
        private NLog.Logger logger;

        public ConsumptionNLog()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Debug(string message, params object[] args)
        {
            logger.Debug(message, args);
        }

        public void Debug(Exception exception, string message)
        {
            logger.Debug(exception, message);
        }

        public void Error(Exception exception, string message)
        {
            logger.Error(exception, message);
        }

        public void Error(string message, params object[] args)
        {
            logger.Error(message, args);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            logger.Info(message, args);
        }

        public void Info(Exception exception, string message)
        {
            logger.Info(exception, message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Warn(string message, params object[] args)
        {
            logger.Warn(message, args);
        }

        public void Warn(Exception exception, string message)
        {
            logger.Warn(exception, message);
        }
    }
}
