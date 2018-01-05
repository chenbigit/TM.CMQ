using CHENBI.CMQ.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHENBI.CMQ
{
    public class Topic
    {
        // topic name 
        protected string topicName;
        // cmq client 
        protected CMQClient client;

        /**
		 * construct .
		 *
		 * @param topicName String 
		 * @param client  CMQClient
		 */
        internal Topic(string topicName, CMQClient client)
        {
            this.topicName = topicName;
            this.client = client;
        }


        /**
	     * TODO set topic attributes
	     *
	     * @param maxMsgSize  int 
	     * @throws Exception  
	     */
        public void SetTopicAttributes(int maxMsgSize)
        {
            if (maxMsgSize < 0 || maxMsgSize > 65536)
                throw new CMQClientException("Invalid parameter maxMsgSize < 0 or maxMsgSize > 65536");
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param["topicName"] = this.topicName;
            if (maxMsgSize > 0)
                param["maxMsgSize"] = maxMsgSize.ToString();

            string result = this.client.Call("SetTopicAttributes", param);
            BaseMessage jsonObj = result.ToObject<BaseMessage>();
            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
        }


        /**
        * TODO get topic attributes.
        *
        * @return  return topic meta object
        * @throws Exception
        */
        public TopicMeta GetTopicAttributes()
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param["topicName"] = this.topicName;
            string result = this.client.Call("GetTopicAttributes", param);
            TopicMeta meta = result.ToObject<TopicMeta>();
            if (meta.Code != 0)
                throw new CMQServerException(meta.Code, meta.Message, meta.RequestId);
            return meta;
        }


        /**
	     * publish message without  tags.
	     *
	     * @param message  String
	     * @return msgId, String
	     * @throws Exception
	     */
        public string PublishMessage(string message)
        {
            return PublishMessage(message, null, null);
        }
        public string PublishMessage(string message, string routingKey)
        {
            return PublishMessage(message, null, routingKey);
        }

        
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="msg">String message body</param>
        /// <param name="vTagList">message tag</param>
        /// <param name="routingKey"></param>
        /// <returns>msgId String </returns>
        public string PublishMessage(String msg, List<String> vTagList, String routingKey)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param["topicName"] = this.topicName;
            param["msgBody"] = msg;
            if (routingKey != null)
                param["routingKey"] = msg;
            if (vTagList != null)
            {
                for (int i = 0; i < vTagList.Count(); ++i)
                {
                    param["msgTag." + (i + 1)] = vTagList[i];
                    //param.put("msgTag." + Integer.toString(i + 1), vTagList.get(i));
                }
            }
            String result = this.client.Call("PublishMessage", param);
            SendMessage jsonObj = result.ToObject<SendMessage>();
            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            return jsonObj.MsgId;
        }

        /**
      * TODO batch publish message without tags.
      *
      * @param vMsgList Vector<String> message array
      * @return msgId Vector<String> message id array
      * @throws Exception 
      */
        public IList<string> BatchPublishMessage(List<String> vMsgList)
        {
            return BatchPublishMessage(vMsgList, null, null);
        }
        public IList<string> BatchPublishMessage(List<String> vMsgList, String routingKey)
        {
            return BatchPublishMessage(vMsgList, null, routingKey);
        }

        /**
         * batch publish message 
         *
         * @param vMsgList   message array
         * @param vTagList   message tag array
         * @return message handles array
         * @throws Exception
         */
        public IList<string> BatchPublishMessage(List<string> vMsgList, List<string> vTagList, string routingKey)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param["topicName"] = this.topicName;
            if (routingKey != null)
                param["routingKey"] = routingKey;
            if (vMsgList != null)
            {
                for (int i = 0; i < vMsgList.Count(); ++i)
                    param["msgBody." + (i + 1)] = vMsgList[i];
            }
            if (vTagList != null)
            {
                for (int i = 0; i < vTagList.Count(); ++i)
                    param["msgTag." + (i + 1)] = vTagList[i];
            }

            String result = this.client.Call("BatchPublishMessage", param);
            BatchPublishMessage jsonObj = result.ToObject<BatchPublishMessage>();
            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            var jsonArray = jsonObj.MsgList;
            IList<string> vmsgId = new List<string>();
            for (int i = 0; i < jsonArray.Count(); i++)
            {
                var obj = jsonArray[i];
                vmsgId.Add(obj.MsgId);
            }
            return vmsgId;
        }

        /**
	     * TODO list subscription by topic.
	     *
	     * @param totalCount           int
	     * @param offset               int
	     * @param limit                int
	     * @param searchWord           String
	     * @param vSubscriptionList    List<String>
	     * @return totalCount          int
	     * @throws Exception
	     */
        public int ListSubscription(int offset, int limit, string searchWord, List<String> vSubscriptionList)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();

            param["topicName"] = this.topicName;
            if (!string.IsNullOrEmpty(searchWord))
                param["searchWord"] = searchWord;
            if (offset >= 0)
                param["offset"] = offset.ToString();
            if (limit > 0)
                param["limit"] = limit.ToString();

            string result = this.client.Call("ListSubscriptionByTopic", param);
            ListSubscriptionByTopic jsonObj = result.ToObject<ListSubscriptionByTopic>();


            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);

            int totalCount = jsonObj.TotalCount;
            var jsonArray = jsonObj.SubscriptionList;
            for (int i = 0; i < jsonArray.Count(); i++)
            {
                var obj = jsonArray[i];
                vSubscriptionList.Add(obj.SubscriptionName);
            }
            return totalCount;
        }

    }
}
