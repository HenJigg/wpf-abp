namespace Consumption.Shared.HttpContact
{
    public class WebResult
    {
        /// <summary>
        /// 后台消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// //返回状态
        /// </summary>
        public int StatusCode { get; set; }

        public object Result { get; set; } 
    }

    public class WebResult<T> : WebResult
    {
        public new T Result { get; set; }
    }
}
