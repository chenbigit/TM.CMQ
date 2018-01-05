
namespace CHENBI.CMQ {
    /// <summary>
    /// 基本消息格式
    /// </summary>
    public class BaseMessage
    {
        /// <summary>
        /// 0：表示成功，others：错误，详细错误见下表。
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 错误提示信息。
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 服务器生成的请求Id。出现服务器内部错误时，用户可提交此 Id 给后台定位问题。
        /// </summary>
        public string RequestId { get; set; }
    }
}
