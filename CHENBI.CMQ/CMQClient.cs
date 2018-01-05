using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHENBI.CMQ
{
    /// <summary>
    /// CMQ 客户端
    /// </summary>
    public class CMQClient
    {
        protected string endpoint;
        protected string path;
        protected string secretId;
        protected string secretKey;
        protected string method;
        protected string signMethod;
        protected CMQHttp cmqHttp;
        protected const string _UpperString = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="path"></param>
        /// <param name="secretId"></param>
        /// <param name="secretKey"></param>
        /// <param name="method"></param>
        public CMQClient(string endpoint, string path, string secretId, string secretKey, string method)
        {
            this.endpoint = endpoint;
            this.path = path;
            this.secretId = secretId;
            this.secretKey = secretKey;
            this.method = method;
            this.signMethod = "sha256";
            this.cmqHttp = new CMQHttp();
        }
        /// <summary>
        /// 构造签名
        /// </summary>
        public virtual string SignMethod
        {
            set
            {
                if (value == "sha1" || value == "sha256")
                {
                    this.signMethod = value;
                }
                else
                {
                    throw new CMQClientException("Only support sha256 or sha1");
                }
            }
        }
        /// <summary>
        /// 呼叫APIURL
        /// </summary>
        /// <param name="action"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual string Call(string action, SortedDictionary<string, string> param)
        {
            string rsp = "";
            try
            {

                #region 1-签名生成
                param["Action"] = action;
                param["Region"] = "bj";
                param["Timestamp"] = CMQTool.UnixTime();
                param["Nonce"] = Convert.ToString((new Random()).Next(int.MaxValue));
                if (this.signMethod == "sha256")
                {
                    param["SignatureMethod"] = "HmacSHA256";  //HmacSHA256
                }
                else
                {
                    param["SignatureMethod"] = "HmacSHA1";
                }
                param["SecretId"] = this.secretId;

                string host = "";
                if (this.endpoint.StartsWith("https"))
                {
                    host = this.endpoint.Substring(8);
                }
                else
                {
                    host = this.endpoint.Substring(7);
                }
                string src = "";
                src += this.method + host + this.path + "?";
                //进行排序
                IList<string> keyList = new List<string>();
                foreach (string key in param.Keys)
                {
                    keyList.Add(key);
                }
                List<string> upperKeyList = (from v1 in keyList
                                             where _UpperString.Contains(v1[0])
                                             orderby v1 ascending
                                             select v1).ToList();
                List<string> lowerKeyList = (from v1 in keyList
                                             where _UpperString.ToLower().Contains(v1[0])
                                             orderby v1 ascending
                                             select v1).ToList();

                //进行拼接
                bool flag = false;
                foreach (var key in upperKeyList)
                {
                    if (flag)
                    {
                        src += "&";
                    }
                    src += key.Replace("_", ".") + "=" + param[key];
                    flag = true;
                }
                foreach (var key in lowerKeyList)
                {
                    src += "&";
                    src += key.Replace("_", ".") + "=" + param[key];
                    flag = true;
                }
                param["Signature"] = HttpUtility.UrlEncode(CMQTool.Sign(src, this.secretKey, this.signMethod), System.Text.Encoding.UTF8);
                #endregion

                string url = "";
                string req = "";
                if (this.method.Equals("GET"))
                {
                    url = this.endpoint + this.path + "?";
                    flag = false;
                    foreach (string key in param.Keys)
                    {
                        if (flag)
                        {
                            url += "&";
                        }
                        //url += key + "=" + System.Web.HttpUtility.UrlEncode(param[key], System.Text.Encoding.UTF8);
                        url += key + "=" + param[key];
                        flag = true;
                    }
                    if (url.Length > 2048)
                    {
                        throw new CMQClientException("URL length is larger than 2K when use GET method");
                    }
                }
                else
                {
                    url = this.endpoint + this.path;
                    flag = false;
                    foreach (string key in param.Keys)
                    {
                        if (flag)
                        {
                            req += "&";
                        }
                        //req += key + "=" + System.Web.HttpUtility.UrlEncode(param[key], System.Text.Encoding.UTF8);
                        req += key + "=" + param[key];
                        flag = true;
                    }
                }

                //System.out.println("url:"+url);
                int userTimeout = 10000;
                if (param.ContainsKey("UserpollingWaitSeconds"))
                {
                    userTimeout = Convert.ToInt32(param["UserpollingWaitSeconds"]);
                }
                rsp = this.cmqHttp.Request(this.method, url, req, userTimeout);
            }
            catch (Exception e)
            {
                throw e;
            }
            return rsp;
        }
    }

}