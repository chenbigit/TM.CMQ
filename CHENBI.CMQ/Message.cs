using System.Collections.Generic;

namespace CHENBI.CMQ {
    /// <summary>
    /// 队列中的一条消息
    /// </summary>
    public class QueueMessage:BaseMessage
    {
        public QueueMessage() {

        }
        /// <summary>
        /// 本次消费的消息唯一标识 Id。
        /// </summary>
        public string MsgId;
        /// <summary>
        /// 每次消费返回唯一的消息句柄。用于删除该消息，仅上一次消费时产生的消息句柄能用于删除消息。
        /// </summary>
        public string ReceiptHandle;
        /// <summary>
        /// 本次消费的消息正文。
        /// </summary>
        public string MsgBody;
        /// <summary>
        /// 消费被生产出来，进入队列的时间。返回Unix时间戳，精确到秒。
        /// </summary>
        public long EnqueueTime;
        /// <summary>
        /// 消息的下次可见（可再次被消费）时间。返回Unix时间戳，精确到秒。
        /// </summary>
        public long NextVisibleTime;
        /// <summary>
        /// 第一次消费该消息的时间。返回Unix时间戳，精确到秒。
        /// </summary>
        public long FirstDequeueTime;
        /// <summary>
        /// 消息被消费的次数。
        /// </summary>
        public int DequeueCount;
    }
    public class BatchQueueMessage:BaseMessage
    {

        /// <summary>
        /// message信息列表，每个元素是一条消息的具体信息。
        /// </summary>
        public QueueMessage[] MsgInfoList { get; set; }
    }
}