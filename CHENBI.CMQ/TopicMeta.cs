using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHENBI.CMQ
{
    public class TopicMeta : BaseMessage
    {
        // 当前该主题的消息堆积数
        public int msgCount { get; set; } 
        // 消息最大长度，取值范围1024-65536 Byte（即1-64K），默认65536
        public int maxMsgSize { get; set; } 
        //消息在主题中最长存活时间，从发送到该主题开始经过此参数指定的时间后，
        //不论消息是否被成功推送给用户都将被删除，单位为秒。固定为一天，该属性不能修改。
        public int msgRetentionSeconds { get; set; } 
        //创建时间
        public int createTime { get; set; }
        //修改属性信息最近时间
        public int lastModifyTime { get; set; }
        public int loggingEnabled { get; set; }
        public int filterType { get; set; }

        public TopicMeta()
        {
            msgCount = 0;
            maxMsgSize = 65536;
            msgRetentionSeconds = 86400;
            createTime = 0;
            lastModifyTime = 0;
            loggingEnabled = 0;
            filterType = 1;
        }
    }
}
