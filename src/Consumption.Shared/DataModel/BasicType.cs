namespace Consumption.Shared.DataModel
{
    /// <summary>
    /// 字典类型
    /// </summary>
    public class BasicType : BaseEntity
    {
        /// <summary>
        /// 字典代码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        public string TypeName { get; set; }
    }
}
