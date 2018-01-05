using CHENBI.CMQ.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace CHENBI.CMQ
{
    /// <summary>
    /// 订阅
    /// </summary>
    public class Subscription
    {
        protected string topicName;
        protected string subscriptionName;
        protected CMQClient client;

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="topicName">主题名字，在单个地域同一帐号下唯一。主题名称是一个不超过 64 个字符的字符串，必须以字母为首字符，剩余部分可以包含字母、数字和横划线(-)。</param>
        /// <param name="subscriptionName">订阅名字，在单个地域同一帐号的同一主题下唯一。订阅名称是一个不超过 64 个字符的字符串，必须以字母为首字符，剩余部分可以包含字母、数字和横划线(-)。</param>
        /// <param name="client"></param>
        internal Subscription(String topicName, String subscriptionName, CMQClient client)
        {
            this.topicName = topicName;
            this.subscriptionName = subscriptionName;
            this.client = client;
        }

        /// <summary>
        /// 清空订阅标签
        /// </summary>
        public void ClearFilterTags()
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param["topicName"] = this.topicName;
            param["subscriptionName"] = this.subscriptionName;
            String result = this.client.Call("ClearSUbscriptionFIlterTags", param);

            BaseMessage jsonObj = result.ToObject<BaseMessage>();

            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
        }

        /// <summary>
        /// 设置订阅属性
        /// </summary>
        /// <param name="meta"></param>
        public void SetSubscriptionAttributes(SubscriptionMeta meta)
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param["topicName"] = this.topicName;
            param["subscriptionName"] = this.subscriptionName;

            if (!string.IsNullOrEmpty(meta.NotifyStrategy))
                param["notifyStrategy"] = meta.NotifyStrategy;
            if (!string.IsNullOrEmpty(meta.NotifyContentFormat))
                param["notifyContentFormat"] = meta.NotifyContentFormat;
            if (meta.FilterTag != null)
            {
                int n = 1;
                foreach (var flag in meta.FilterTag)
                {
                    param["filterTag." + n] = flag;
                    ++n;
                }
            }
            if (meta.bindingKey != null)
            {
                int n = 1;

                foreach (var flag in meta.bindingKey)
                {
                    param["bindingKey." + n] = flag;
                    ++n;
                }
            }
            String result = this.client.Call("SetSubscriptionAttributes", param);
            BaseMessage jsonObj = result.ToObject<BaseMessage>();

            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
        }


        /// <summary>
        /// 获取订阅属性
        /// </summary>
        /// <returns></returns>
        public SubscriptionMeta GetSubscriptionAttributes()
        {
            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param["topicName"] = this.topicName;
            param["subscriptionName"] = this.subscriptionName;
            String result = this.client.Call("GetSubscriptionAttributes", param);
            SubscriptionAttributes jsonObj = result.ToObject<SubscriptionAttributes>();
            if (jsonObj.Code != 0)
                throw new CMQServerException(jsonObj.Code, jsonObj.Message, jsonObj.RequestId);
            SubscriptionMeta meta = new SubscriptionMeta();
            meta.FilterTag = new List<String>();
            if (!string.IsNullOrEmpty(jsonObj.Endpoint))
                meta.Endpoint = jsonObj.Endpoint;
            if (!string.IsNullOrEmpty(jsonObj.NotifyStrategy))
                meta.NotifyStrategy = jsonObj.NotifyStrategy;
            if (!string.IsNullOrEmpty(jsonObj.NotifyContentFormat))
                meta.NotifyContentFormat = jsonObj.NotifyContentFormat;
            if (!string.IsNullOrEmpty(jsonObj.Protocol))
                meta.Protocal = jsonObj.Protocol;
            if (jsonObj.CreateTime.HasValue)
                meta.CreateTime = jsonObj.CreateTime.Value;
            if (jsonObj.LastModifyTime.HasValue)
                meta.LastModifyTime = jsonObj.LastModifyTime.Value;
            if (jsonObj.MsgCount.HasValue)
                meta.msgCount = jsonObj.MsgCount.Value;
            if (null != jsonObj.FilterTag && jsonObj.FilterTag.Count > 0)
            {
                var jsonArray = jsonObj.FilterTag;
                for (int i = 0; i < jsonArray.Count(); i++)
                {
                    var obj = jsonArray[i];
                    meta.FilterTag.Add(obj);
                }
            }
            if (null != jsonObj.BindingKey && jsonObj.BindingKey.Count > 0)
            {
                var jsonArray = jsonObj.BindingKey;
                for (int i = 0; i < jsonArray.Count(); i++)
                {
                    var obj = jsonArray[i];
                    meta.bindingKey.Add(obj);
                }
            }
            return meta;
        }
    }
}
