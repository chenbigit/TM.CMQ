using System.Collections.Generic;


namespace CHENBI.CMQ {
    /// <summary>
    /// 批量删除消息
    /// </summary>
    public class BatchDeleteMessage : BaseMessage
    {
        /// <summary>
        /// 无法成功删除的错误列表。每个元素列出了消息无法成功被删除的错误及原因。
        /// </summary>
        public List<DeleteMessageError> ErrorList { get; set; }
    }
    public class DeleteMessageError {
        /// <summary>
        /// 0：表示成功，others：错误，详细错误见下表。
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 错误提示信息。
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 对应删除失败的消息句柄。
        /// </summary>
        public int ReceiptHandle { get; set; }
    }
}
