namespace Consumption.Shared.HttpContact.Response
{
    /// <summary>
    /// 请求返回定义类
    /// </summary>
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = "")
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }

        public ApiResponse(int statusCode, object result = null)
        {
            this.StatusCode = statusCode;
            this.Result = result;
        }

        /// <summary>
        /// 后台消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// //返回状态
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public object Result { get; set; }
    }
}
