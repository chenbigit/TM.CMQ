

namespace CHENBI.CMQ
{
    /// <summary>
    /// 主题清单
    /// </summary>
    public class ListTopic : BaseMessage
    {
        /// <summary>
        /// 主题总个数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 主题清单
        /// </summary>
        public TopicDetail[] TopicList { get; set; }
    }
    public class TopicDetail
    {
        /// <summary>
        /// 主题Id
        /// </summary>
        public string TopicId { get; set; }
        /// <summary>
        /// 主题名称
        /// </summary>
        public string TopicName { get; set; }
    }
}
