using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHENBI.CMQ {
    /// <summary>
    /// 向队列发送消息
    /// </summary>
    public class BatchSendMessage : BaseMessage
    {
        /// <summary>
        /// 服务器生成消息的唯一标识 Id列表，每个元素是一条消息的信息。
        /// </summary>
        public List<string> MsgList { get; set; }
    }


}
