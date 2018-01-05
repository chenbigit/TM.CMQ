using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHENBI.CMQ {
    /// <summary>
    /// 队列清单
    /// </summary>
    public class ListQueue : BaseMessage
    {
        /// <summary>
        /// 队列总个数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 队列清单
        /// </summary>
        public QueueInfo[] QueueList { get; set; }
    }
    public class QueueInfo
    {
        /// <summary>
        /// 队列Id
        /// </summary>
        public string QueueId { get; set; }
        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }
    }
}
