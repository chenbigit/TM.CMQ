
namespace CHENBI.CMQ {
    /// <summary>
    /// 向队列发送消息
    /// </summary>
    public class SendMessage : BaseMessage
    {
        /// <summary>
        /// 服务器生成消息的唯一标识 Id。
        /// </summary>
        public string MsgId { get; set; }
    }

}
