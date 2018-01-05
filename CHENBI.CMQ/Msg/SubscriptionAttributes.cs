using System.Collections.Generic;

namespace CHENBI.CMQ
{
    /// <summary>
    /// 获取订阅属性
    /// </summary>
    public class SubscriptionAttributes: BaseMessage
    {
        /// <summary>
        /// 订阅拥有者的appid
        /// </summary>
        public string TopicOwner { get; set; }
        /// <summary>
        /// 该订阅待投递的消息数。
        /// </summary>
        public int? MsgCount { get; set; }
        /// <summary>
        /// 订阅的协议，目前支持两种协议：http、queue。使用http协议，用户需自己搭建接受消息的web server。
        /// 使用queue，消息会自动推送到CMQ queue，用户可以并发地拉取消息。
        /// </summary>
        public string Protocol { get; set; }
        /// <summary>
        /// 接收通知的endpoint，根据协议protocol区分：
        /// 对于http，endpoint必须以“http://”开头，host可以是域名或IP；对于queue，则填queueName。
        /// </summary>
        public string Endpoint { get; set; }
        /// <summary>
        /// 向endpoint推送消息出现错误时，CMQ推送服务器的重试策略。
        /// 取值有：
        /// 1）BACKOFF_RETRY，退避重试。
        /// 每隔一定时间重试一次，重试够一定次数后，就把该消息丢弃，继续推送下一条消息；
        /// 2）EXPONENTIAL_DECAY_RETRY，指数衰退重试。
        /// 每次重试的间隔是指数递增的，例如开始1s，后面是2s，4s，8s...由于Topic消息的周期是一天，
        /// 所以最多重试一天就把消息丢弃。默认值是EXPONENTIAL_DECAY_RETRY。
        /// </summary>
        public string NotifyStrategy { get; set; }
        /// <summary>
        /// 推送内容的格式。取值：1）JSON；2）SIMPLIFIED，即raw格式。
        /// 如果protocol是queue，则取值必须为SIMPLIFIED。如果protocol是http，两个值均可以，默认值是JSON。
        /// </summary>
        public string NotifyContentFormat { get; set; }
        /// <summary>
        /// 订阅的创建时间。返回Unix时间戳，精确到秒。
        /// </summary>
        public int? CreateTime { get; set; }
        /// <summary>
        /// 最后一次修改订阅属性的时间。返回Unix时间戳，精确到秒。
        /// </summary>
        public int? LastModifyTime { get; set; }
        /// <summary>
        /// 表示订阅接收消息的过滤策略。
        /// </summary>
        public List<string> BindingKey { get; set; }

        /// <summary>
        /// 暂时查不到
        /// </summary>
        public List<string> FilterTag { get; set; }
    }
}
