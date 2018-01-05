using CHENBI.CMQ.Utility;
using System;
using System.Collections.Generic;

namespace CHENBI.CMQ
{
    /// <summary>
    /// CMQ -Account 
    /// </summary>
    public class Account
    {
        protected internal CMQClient client;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="secretId">密钥Id</param>
        /// <param name="secretKey">密钥Key</param>
        /// <param name="endpoint">请求域名，例如：广州访问点->http://cmq-queue-gz.api.tencentyun.com</param>
        /// <param name="path">例如：/v2/index.php</param>
        /// <param name="method">POST/GET,默认为POST</param>
        public Account(string secretId, string secretKey, string endpoint = "http://cmq-queue-gz.api.tencentyun.com", string path = "/v2/index.php", string method = "POST")
        {
            this.client = new CMQClient(endpoint, path, secretId, secretKey, method);
        }

        public virtual string SignMethod
        {
            set
            {
                this.client.SignMethod = value;
            }
        }
        /// <summary>
        /// 创建一个队列
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="meta">        QueueMeta class object </param>
        /// <exception cref="Exception"> </exception>
        /// <exception cref="CMQClientException"> </exception>
        /// <exception cref="CMQServerException"> </exception>
        public virtual void CreateQueue(string queueName, QueueMeta meta)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            if (queueName.Equals(""))
            {
                throw new CMQClientException("Invalid parameter:queueName is empty");
            }
            else
            {
                param["queueName"] = queueName;
            }

            if (meta.MaxMsgHeapNum > 0)
            {
                param["maxMsgHeapNum"] = Convert.ToString(meta.MaxMsgHeapNum);
            }
            if (meta.PollingWaitSeconds > 0)
            {
                param["pollingWaitSeconds"] = Convert.ToString(meta.PollingWaitSeconds);
            }
            if (meta.VisibilityTimeout > 0)
            {
                param["visibilityTimeout"] = Convert.ToString(meta.VisibilityTimeout);
            }
            if (meta.MaxMsgSize > 0)
            {
                param["maxMsgSize"] = Convert.ToString(meta.MaxMsgSize);
            }
            if (meta.MsgRetentionSeconds > 0)
            {
                param["msgRetentionSeconds"] = Convert.ToString(meta.MsgRetentionSeconds);
            }
            if (meta.RewindSeconds > 0)
            {
                param["rewindSeconds"] = Convert.ToString(meta.RewindSeconds);
            }

            string result = this.client.Call("CreateQueue", param);
            CreateQueue jsonObj = result.ToObject<CHENBI.CMQ.CreateQueue>();//  JsonConvert.DeserializeObject<Msg.CreateQueue>(result);
            if (jsonObj.Code != 0)
            {
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            }
        }

        /// <summary>
        /// 删除一个队列
        /// </summary>
        /// <param name="queueName">   String queue name </param>
        /// <exception cref="CMQClientException"> </exception>
        /// <exception cref="CMQServerException"> </exception>
        public virtual void DeleteQueue(string queueName)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            if (queueName.Equals(""))
            {
                throw new CMQClientException("Invalid parameter:queueName is empty");
            }
            else
            {
                param["queueName"] = queueName;
            }
            string result = this.client.Call("DeleteQueue", param);
            BaseMessage jsonObj = result.ToObject<BaseMessage>();// JsonConvert.DeserializeObject<Msg.Base>(result);
            if (jsonObj.Code != 0)
            {
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            }
        }


        /// <summary>
        /// 获取队列列表
        /// </summary>
        /// <param name="searchWord"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="queueList"></param>
        /// <returns>totalCount int</returns>
        public virtual int ListQueue(string searchWord, int offset, int limit, List<QueueInfo> queueList)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            if (!searchWord.Equals(""))
            {
                param["searchWord"] = searchWord;
            }
            if (offset >= 0)
            {
                param["offset"] = offset.ToString();
            }
            if (limit > 0)
            {
                param["limit"] = limit.ToString();
            }

            string result = this.client.Call("ListQueue", param);
            ListQueue jsonObj = result.ToObject<ListQueue>(); 
            if (jsonObj.Code != 0)
            {
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            }
            if (queueList == null) { queueList = new List<QueueInfo>(); }
            queueList.Clear();
            foreach (var model in jsonObj.QueueList)
            {
                queueList.Add(model);
            }
            return jsonObj.TotalCount;
        }

        /// <summary>
        /// 获取一个队列 
        /// </summary>
        /// <param name="queueName">  String </param>
        /// <returns> Queue object
        ///  </returns>
        public virtual Queue GetQueue(string queueName)
        {
            return new Queue(queueName, this.client);
        }


        /// <summary>
        /// 获取主题
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns></returns>
        public Topic GetTopic(String topicName)
        {
            return new Topic(topicName, this.client);
        }


        /// <summary>
        /// 创建主题
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="maxMsgSize"></param>
        public void CreateTopic(string topicName, int maxMsgSize)
        {
            CreateTopic(topicName, maxMsgSize, 1);
        }

        /// <summary>
        /// 创建主题
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="maxMsgSize"></param>
        /// <param name="filterType"></param>
        public void CreateTopic(string topicName, int maxMsgSize, int filterType)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            if (string.IsNullOrEmpty(topicName))
                throw new CMQClientException("Invalid parameter:topicName is empty");
            else
                param["topicName"] = topicName;  //param.put("topicName",topicName);

            param["filterType"] = filterType.ToString(); // param.put("filterType",Integer.toString(filterType));

            if (maxMsgSize < 1 || maxMsgSize > 65536)
                throw new CMQClientException("Invalid parameter: maxMsgSize > 65536 or maxMsgSize < 1");
            param["maxMsgSize"] = maxMsgSize.ToString();
            String result = this.client.Call("CreateTopic", param);
            BaseMessage jsonObj = result.ToObject<BaseMessage>();
            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
        }



        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="topicName"></param>
        public void DeleteTopic(string topicName)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            if (string.IsNullOrEmpty(topicName))
                throw new CMQClientException("Invalid parameter:topicName is empty");
            else
                param["topicName"] = topicName;  //param.put("topicName",topicName);

            String result = this.client.Call("DeleteTopic", param);
            BaseMessage jsonObj = result.ToObject<BaseMessage>();
            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
        }


        /// <summary>
        /// 出队主题
        /// </summary>
        /// <param name="searchWord"></param>
        /// <param name="vTopicList"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public int ListTopic(string searchWord, List<String> vTopicList, int offset, int limit)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            if (string.IsNullOrEmpty(searchWord))
                param["searchWord"] = searchWord;
            if (offset >= 0)
                param["offset"] = offset.ToString();
            if (limit > 0)
                param["limit"] = limit.ToString();
            string result = this.client.Call("ListTopic", param);
            ListTopic jsonObj = result.ToObject<ListTopic>();
            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            int totalCount = jsonObj.TotalCount;
            var jsonArray = jsonObj.TopicList;
            vTopicList.Clear();
            for (int i = 0; i < jsonArray.Length; i++)
            {
                var obj = jsonArray[i];
                vTopicList.Add(obj.TopicName);
            }
            return totalCount;
        }



        /// <summary>
        /// 创建订阅
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="subscriptionName"></param>
        /// <param name="Endpoint"></param>
        /// <param name="Protocal"></param>
        public void CreateSubscribe(string topicName, string subscriptionName, string Endpoint, string Protocal)
        {
            CreateSubscribe(topicName, subscriptionName, Endpoint, Protocal, null, null, "BACKOFF_RETRY", "JSON");

        }

        /// <summary>
        /// 创建订阅
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="subscriptionName"></param>
        /// <param name="endpoint"></param>
        /// <param name="protocal"></param>
        /// <param name="filterTag"></param>
        /// <param name="bindingKey"></param>
        /// <param name="notifyStrategy"></param>
        /// <param name="notifyContentFormat"></param>
        public void CreateSubscribe(string topicName, string subscriptionName, string endpoint, string protocal,
                 List<string> filterTag, List<string> bindingKey, string notifyStrategy, string notifyContentFormat)
        {

            if (filterTag != null && filterTag.Count > 5)
                throw new CMQClientException("Invalid parameter: Tag number > 5");

            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            if (!string.IsNullOrEmpty(topicName))
                throw new CMQClientException("Invalid parameter:topicName is empty");
            param["topicName"] = topicName;

            if (!string.IsNullOrEmpty(subscriptionName))
                throw new CMQClientException("Invalid parameter:subscriptionName is empty");
            param["subscriptionName"] = subscriptionName;

            if (!string.IsNullOrEmpty(endpoint))
                throw new CMQClientException("Invalid parameter:endpoint is empty");
            param["endpoint"] = endpoint;

            if (!string.IsNullOrEmpty(protocal))
                throw new CMQClientException("Invalid parameter:protocal is empty");
            param["protocal"] = endpoint;

            if (!string.IsNullOrEmpty(notifyStrategy))
                throw new CMQClientException("Invalid parameter:notifyStrategy is empty");
            param["notifyStrategy"] = endpoint;

            if (!string.IsNullOrEmpty(notifyContentFormat))
                throw new CMQClientException("Invalid parameter:notifyStrategy is empty");
            param["notifyContentFormat"] = notifyContentFormat;
            if (filterTag != null)
            {
                for (int i = 0; i < filterTag.Count; ++i)
                    param["filterTag." + (i + 1)] = filterTag[i];
            }
            if (bindingKey != null)
            {
                for (int i = 0; i < bindingKey.Count; ++i)
                    param["bindingKey." + (i + 1)] = bindingKey[i];
            }

            String result = this.client.Call("Subscribe", param);
            BaseMessage jsonObj = result.ToObject<BaseMessage>();
            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
        }


        /// <summary>
        /// 删除订阅
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="subscriptionName"></param>
        public void DeleteSubscribe(string topicName, string subscriptionName)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            if (!string.IsNullOrEmpty(topicName))
                throw new CMQClientException("Invalid parameter:topicName is empty");
            param["topicName"] = topicName;

            if (!string.IsNullOrEmpty(subscriptionName))
                throw new CMQClientException("Invalid parameter:subscriptionName is empty");
            param["subscriptionName"] = subscriptionName;

            string result = this.client.Call("Unsubscribe", param);
            BaseMessage jsonObj = result.ToObject<BaseMessage>();
            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
        }


        /// <summary>
        /// 获取订阅
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="subscriptionName"></param>
        /// <returns></returns>
        public Subscription getSubscription(string topicName, string subscriptionName)
        {
            return new Subscription(topicName, subscriptionName, this.client);
        }
    }

}