namespace CHENBI.CMQ
{
    /// <summary>
    /// 队列元数据，队列基本属性
    /// </summary>
    public class QueueMeta:BaseMessage
    {
        public QueueMeta() {
            this.PollingWaitSeconds = 0;
            this.VisibilityTimeout = 30;
            this.MaxMsgSize = 65536;
            this.MsgRetentionSeconds = 345600;
            this.MaxMsgHeapNum = -1;
            this.CreateTime = -1;
            this.LastModifyTime = -1;
            this.ActiveMsgNum = -1;
            this.InactiveMsgNum = -1;
        }
        /// <summary>
        /// 最大堆积消息数。取值范围在公测期间为 1,000,000 - 10,000,000，正式上线后范围可达到 1000,000-1000,000,000。默认取值在公测期间为 10,000,000，正式上线后为 100,000,000。
        /// </summary>
        public int MaxMsgHeapNum { get; set; }
        /// <summary>
        /// 消息接收长轮询等待时间。取值范围0-30秒，默认值0。
        /// </summary>
        public int PollingWaitSeconds { get; set; }
        /// <summary>
        /// 消息可见性超时。取值范围1-43200秒（即12小时内），默认值30。
        /// </summary>
        public int VisibilityTimeout { get; set; }
        /// <summary>
        /// 消息最大长度。取值范围1024-65536 Byte（即1-64K），默认值65536。
        /// </summary>
        public int MaxMsgSize { get; set; }
        /// <summary>
        /// 消息保留周期。取值范围60-1296000秒（1min-15天），默认值345600 (4 天)。
        /// </summary>
        public int MsgRetentionSeconds { get; set; }
        /// <summary>
        /// 队列的创建时间。返回Unix时间戳，精确到秒。
        /// </summary>
        public int CreateTime { get; set; }
        /// <summary>
        /// 最后一次修改队列属性的时间。返回Unix时间戳，精确到秒。
        /// </summary>
        public int LastModifyTime { get; set; }
        /// <summary>
        /// 在队列中处于 Active 状态（不处于被消费状态）的消息总数，为近似值。
        /// </summary>
        public int ActiveMsgNum { get; set; }
        /// <summary>
        /// 在队列中处于 Inactive 状态（正处于被消费状态）的消息总数，为近似值。
        /// </summary>
        public int InactiveMsgNum { get; set; }
        /// <summary>
        /// 已调用DelMsg接口删除，但还在回溯保留时间内的消息数量。
        /// </summary>
        public int RewindmsgNum { get; set; }
        /// <summary>
        /// 消息最小未消费时间，单位为秒
        /// </summary>
        public int MinMsgTime { get; set; }
        /// <summary>
        /// 延时消息数量
        /// </summary>
        public int DelayMsgNum { get; set; }
        /// <summary>
        /// 最长消息回溯时间,单位秒 
        /// </summary>
        public int RewindSeconds { get; set; }

    }

}