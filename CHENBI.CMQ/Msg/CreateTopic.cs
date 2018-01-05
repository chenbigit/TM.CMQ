using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHENBI.CMQ {
    /// <summary>
    /// 创建一个主题后返回的消息
    /// </summary>
    public class CreateTopic : BaseMessage
    {
        /// <summary>
        /// 主题的Id。
        /// </summary>
        public string TopicId { get; set; }
    }
}
