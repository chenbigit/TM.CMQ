using CHENBI.CMQ.Utility;
using System;
using System.Collections.Generic;

namespace CHENBI.CMQ {
    /// <summary>
    /// 队列
    /// </summary>
    public class Queue
    {
        protected string queueName;
        protected CMQClient client;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="client"></param>
        internal Queue(string queueName, CMQClient client)
        {
            this.queueName = queueName;
            this.client = client;
        }
        /// <summary>
        /// 获取或设置队列的属性/元数据
        /// </summary>
        public virtual QueueMeta QueueAttributes
        {
            set
            {
                SortedDictionary<string, string> param = new SortedDictionary<string, string>();

                param["queueName"] = this.queueName;

                if (value.MaxMsgHeapNum > 0)
                {
                    param["maxMsgHeapNum"] = Convert.ToString(value.MaxMsgHeapNum);
                }
                if (value.PollingWaitSeconds > 0)
                {
                    param["pollingWaitSeconds"] = Convert.ToString(value.PollingWaitSeconds);
                }
                if (value.VisibilityTimeout > 0)
                {
                    param["visibilityTimeout"] = Convert.ToString(value.VisibilityTimeout);
                }
                if (value.MaxMsgSize > 0)
                {
                    param["maxMsgSize"] = Convert.ToString(value.MaxMsgSize);
                }
                if (value.MsgRetentionSeconds > 0)
                {
                    param["msgRetentionSeconds"] = Convert.ToString(value.MsgRetentionSeconds);
                }
                if (value.RewindSeconds > 0)
                {
                    param["rewindSeconds"] = Convert.ToString(value.RewindSeconds);
                }

                string result = this.client.Call("SetQueueAttributes", param);
                QueueMeta jsonObj = result.ToObject<QueueMeta>();//   JsonConvert.DeserializeObject<QueueMeta>(result);
                if (jsonObj.Code != 0)
                {
                    throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
                }
            }
            get
            {
                SortedDictionary<string, string> param = new SortedDictionary<string, string>();
                param["queueName"] = this.queueName;
                string result = this.client.Call("GetQueueAttributes", param);
                QueueMeta jsonObj = result.ToObject<QueueMeta>();// JsonConvert.DeserializeObject<QueueMeta>(result);
                if (jsonObj.Code != 0)
                {
                    throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
                }
                return jsonObj;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgBody">消息内容</param>
        /// <returns>返回消息的MsgId</returns>
        public virtual string SendMessage(string msgBody)
        {
            return SendMessage(msgBody, 0);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgBody">消息内容</param>
        /// <param name="delayTime">单位为秒，表示该消息发送到队列后，需要延时多久用户才可见该消息</param>
        /// <returns>返回消息的MsgId</returns>
        public virtual string SendMessage(string msgBody, int delayTime)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();

            param["queueName"] = this.queueName;
            param["msgBody"] = msgBody;
            param["delaySeconds"] = Convert.ToString(delayTime);

            string result = this.client.Call("SendMessage", param);
            SendMessage jsonObj = result.ToObject<SendMessage>(); //JsonConvert.DeserializeObject<Msg.SendMessage>(result);
            if (jsonObj.Code != 0)
            {
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            }
            return jsonObj.MsgId;
        }
        /// <summary>
        /// 批量发送消息
        /// </summary>
        /// <param name="vtMsgBody">消息集合</param>
        /// <returns>消息的MsgId集合</returns>
        public virtual List<string> BatchSendMessage(IList<string> vtMsgBody)
        {
            return BatchSendMessage(vtMsgBody, 0);
        }
        /// <summary>
        /// 向队列批量发送消息
        /// </summary>
        /// <param name="vtMsgBody">消息集合</param>
        /// <param name="delayTime">单位为秒，表示该消息发送到队列后，需要延时多久用户才可见该消息</param>
        /// <returns></returns>
        public virtual List<string> BatchSendMessage(IList<string> vtMsgBody, int delayTime)
        {
            if (vtMsgBody.Count == 0 || vtMsgBody.Count > 16)
            {
                throw new CMQClientException("Error: message size is empty or more than 16");
            }

            SortedDictionary<string, string> param = new SortedDictionary<string, string>();

            param["queueName"] = this.queueName;
            for (int i = 0; i < vtMsgBody.Count; i++)
            {
                string k = "msgBody." + Convert.ToString(i + 1);
                param[k] = vtMsgBody[i];
            }
            param["delaySeconds"] = Convert.ToString(delayTime);

            string result = this.client.Call("BatchSendMessage", param);
            BatchSendMessage jsonObj = result.ToObject<BatchSendMessage>(); //JsonConvert.DeserializeObject<Msg.BatchSendMessage>(result);
            if (jsonObj.Code != 0)
            {
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            }
            return jsonObj.MsgList;
        }

        /// <summary>
        /// 本接口 (ReceiveMessage) 用于消费队列中的一条消息，
        /// ReceiveMessage 操作会将取得的消息状态变成 inactive，inactive 的时间长度由队列属性 visibilityTimeout 指定（详见CreateQueue接口）。 
        /// 建议消费者在 visibilityTimeout 时间内消费成功后需要调用 (batch)DeleteMessage 接口删除该消息，否则该消息将会重新变成为 active 状态，
        /// 此消息又可被消费者重新消费。
        /// </summary>
        /// <param name="pollingWaitSeconds">本次请求的长轮询等待时间。取值范围 0-30 秒，如果不设置该参数，则默认使用队列属性中的pollingWaitSeconds值。</param>
        /// <returns></returns>
        public virtual QueueMessage ReceiveMessage(int pollingWaitSeconds)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();

            param["queueName"] = this.queueName;
            if (pollingWaitSeconds > 0)
            {
                param["UserpollingWaitSeconds"] = Convert.ToString(pollingWaitSeconds * 1000);
                param["pollingWaitSeconds"] = Convert.ToString(pollingWaitSeconds);
            }
            else
            {
                param["UserpollingWaitSeconds"] = Convert.ToString(30000);
            }

            string result = this.client.Call("ReceiveMessage", param);
            QueueMessage jsonObj = result.ToObject<QueueMessage>(); //JsonConvert.DeserializeObject<QueueMessage>(result);
            if (jsonObj.Code != 0)
            {
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            }
            return jsonObj;
        }

        /// <summary>
        /// 批量消费消息
        /// </summary>
        /// <param name="numOfMsg">本次消费的消息数量。取值范围 1-16。</param>
        /// <param name="pollingWaitSeconds">本次请求的长轮询等待时间。取值范围 0-30 秒,如果不设置该参数，则默认使用队列属性中的pollingWaitSeconds值。</param>
        /// <returns>message信息列表，每个元素是一条消息的具体信息。</returns>
        public virtual List<QueueMessage> BatchReceiveMessage(int numOfMsg, int pollingWaitSeconds)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();

            param["queueName"] = this.queueName;
            param["numOfMsg"] = Convert.ToString(numOfMsg);
            if (pollingWaitSeconds > 0)
            {
                param["UserpollingWaitSeconds"] = Convert.ToString(pollingWaitSeconds * 1000);
                param["pollingWaitSeconds"] = Convert.ToString(pollingWaitSeconds);
            }
            else
            {
                param["UserpollingWaitSeconds"] = Convert.ToString(30000);
            }
            string result = this.client.Call("BatchReceiveMessage", param);
            BatchQueueMessage jsonObj = result.ToObject<BatchQueueMessage>(); //JsonConvert.DeserializeObject<BatchQueueMessage>(result);
            if (jsonObj.Code != 0)
            {
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            }

            List<QueueMessage> vtMessage = new List<QueueMessage>();
            foreach (var model in jsonObj.MsgInfoList)
            {
                QueueMessage msg = model;
                msg.Code = jsonObj.Code;
                msg.Message = jsonObj.Message;
                msg.RequestId = jsonObj.RequestId;
                vtMessage.Add(msg);
            }
            return vtMessage;
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="receiptHandle">每次消费返回唯一的消息句柄，用于删除消息。当且仅当消息上次被消费时产生的句柄能用于删除本条消息。</param>
        public virtual void DeleteMessage(string receiptHandle)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();

            param["queueName"] = this.queueName;
            param["receiptHandle"] = receiptHandle;

            string result = this.client.Call("DeleteMessage", param);
            BaseMessage jsonObj = result.ToObject<BaseMessage>(); //JsonConvert.DeserializeObject<Msg.CMQBase>(result);
            if (jsonObj.Code != 0)
            {
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            }
        }

        /// <summary>
        /// 批量删除消息
        /// </summary>
        /// <param name="vtReceiptHandle"></param>
        public virtual void BatchDeleteMessage(IList<string> vtReceiptHandle)
        {
            if (vtReceiptHandle.Count == 0)
            {
                return;
            }
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param["queueName"] = this.queueName;
            for (int i = 0; i < vtReceiptHandle.Count; i++)
            {
                string k = "receiptHandle." + Convert.ToString(i + 1);
                param[k] = vtReceiptHandle[i];
            }

            string result = this.client.Call("BatchDeleteMessage", param);
            BatchDeleteMessage jsonObj = result.ToObject<BatchDeleteMessage>(); //JsonConvert.DeserializeObject<Msg.BatchDeleteMessage>(result);
            if (jsonObj.Code != 0)
            {
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            }
        }

        /**
	     * get topic object
	     * @param topicName String
	     * @return Topic object
	     */
        public Topic GetTopic(string topicName)
        {
            return new Topic(topicName, this.client);
        }

        /**
	     * TODO create topic
	     *
	     * @param topicName   String
	     * @param maxMsgSize  int
         * @param filterType  
	     * @throws Exception
	     */
        public void CreateTopic(String topicName, int maxMsgSize)
        {
            CreateTopic(topicName, maxMsgSize, 1);
        }
        public void CreateTopic(String topicName, int maxMsgSize, int filterType)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            if (string.IsNullOrEmpty(topicName))
                throw new CMQClientException("Invalid parameter:topicName is empty");
            else
                param["topicName"] = topicName;
            param["filterType"] = filterType.ToString();
            if (maxMsgSize < 1 || maxMsgSize > 65536)
                throw new CMQClientException("Invalid parameter: maxMsgSize > 65536 or maxMsgSize < 1");
            param["maxMsgSize"] = maxMsgSize.ToString();
            string result = this.client.Call("CreateTopic", param);
            BaseMessage jsonObj = result.ToObject<BaseMessage>();
            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
        }






    }

}