using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHENBI.CMQ
{
    /// <summary>
    /// 获取订阅列表
    /// 该接口用于列出用户某主题下的订阅列表，可分页获取数据
    /// </summary>
    public class ListSubscriptionByTopic : BaseMessage
    {
        public int TotalCount { get; set; }
        public List<SubscriptionByTopic> SubscriptionList { get; set; }
    }


    public class SubscriptionByTopic
    {
        /// <summary>
        /// 订阅Id。订阅Id在拉取监控数据时会用到。
        /// </summary>
        public string SubscriptionId { get; set; }
        /// <summary>
        /// 订阅名字，在单个地域同一帐号的同一主题下唯一。订阅名称是一个不超过 64 个字符的字符串，必须以字母为首字符，
        /// 剩余部分可以包含字母、数字和横划线(-)。
        /// </summary>
        public string SubscriptionName { get; set; }
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
    }

    //    {
    //    "code": 0,
    //    "message": "",
    //    "requestId": "14534664555",
    //    "totalCount": 2"topicList": [
    //        {
    //            "subscriptionId": "subsc-sdkfl",
    //            "subscriptionName": "test-sub2",
    //            "protocol": "http""endpoint": "http://testhost/testpath"
    //        },
    //        {
    //            "subscriptionId": "subsc-34lasdk",
    //            "subscriptionName": "test-sub1",
    //            "protocol": "queue""endpoint": "test-queue-name"
    //        }
    //    ]
    //}
}
