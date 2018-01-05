using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CHENBI.CMQ
{
    /// <summary>    
    /// 上传数据参数    
    /// </summary>    
    public class UploadEventArgs : EventArgs
    {
        int bytesSent;
        int totalBytes;
        /// <summary>    
        /// 已发送的字节数    
        /// </summary>    
        public int BytesSent
        {
            get { return bytesSent; }
            set { bytesSent = value; }
        }
        /// <summary>    
        /// 总字节数    
        /// </summary>    
        public int TotalBytes
        {
            get { return totalBytes; }
            set { totalBytes = value; }
        }
    }
    /// <summary>    
    /// 下载数据参数    
    /// </summary>    
    public class DownloadEventArgs : EventArgs
    {
        int bytesReceived;
        int totalBytes;
        byte[] receivedData;
        /// <summary>    
        /// 已接收的字节数    
        /// </summary>    
        public int BytesReceived
        {
            get { return bytesReceived; }
            set { bytesReceived = value; }
        }
        /// <summary>    
        /// 总字节数    
        /// </summary>    
        public int TotalBytes
        {
            get { return totalBytes; }
            set { totalBytes = value; }
        }
        /// <summary>    
        /// 当前缓冲区接收的数据    
        /// </summary>    
        public byte[] ReceivedData
        {
            get { return receivedData; }
            set { receivedData = value; }
        }
    }
    public class CMQHttp
    {
        int bufferSize = 15240;
        WebHeaderCollection responseHeaders;

        #region 事件
        public event EventHandler<UploadEventArgs> UploadProgressChanged;
        public event EventHandler<DownloadEventArgs> DownloadProgressChanged;
        #endregion

        #region 属性
        /// <summary>
        /// 代理服务器
        /// </summary>
        public WebProxy Proxy { get; set; }
        /// <summary>    
        /// 获取或设置与请求关联的Cookie容器    
        /// </summary>    
        public CookieContainer CookieContainer { get; set; }
        /// <summary>
        /// 网页编码
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>    
        /// 获取响应头集合    
        /// </summary>    
        public WebHeaderCollection ResponseHeaders
        {
            get { return responseHeaders; }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public CMQHttp()
        {
            this.Encoding = System.Text.Encoding.UTF8;  //默认为Uft-8编码
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="method">POST/GET</param>
        /// <param name="url">提交的地址</param>
        /// <param name="req">POST内容</param>
        /// <param name="userTimeout">超时时间，单位毫秒,默认10秒</param>
        /// <returns></returns>
        public virtual string Request(string method, string url, string req, int userTimeout = 10000)
        {
            string result = "";
            if (method.ToUpper() == "POST")
            {
                result = this.httpPost(url, req, userTimeout);
            }
            else
            {
                result = this.httpGet(url, userTimeout);
            }
            return result;
        }


        #region 私有/受保护的方法
        /// <summary>
        /// 创建一个WebRequest
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="method">POST/GET</param>
        /// <returns></returns>
        protected HttpWebRequest createRequest(string url, string method)
        {
            Uri uri = new Uri(url);

            if (uri.Scheme == "https")
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.checkValidationResult);

            // Set a default policy level for the "http:" and "https" schemes.    
            HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate);
            HttpWebRequest.DefaultCachePolicy = policy;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowAutoRedirect = false;
            request.AllowWriteStreamBuffering = false;
            request.Method = method;
            if (this.Proxy != null)
                request.Proxy = this.Proxy;
            if (this.CookieContainer != null)
                request.CookieContainer = this.CookieContainer;
            request.Accept = "*/*";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)";
            return request;
        }
        /// <summary>
        /// 通过HttpGet 方式获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userTimeout">单位秒，超时时间</param>
        /// <returns></returns>
        public string httpGet(string url, int userTimeout)
        {
            HttpWebRequest request = this.createRequest(url, "GET");
            request.AllowAutoRedirect = true;
            request.Timeout = userTimeout;
            byte[] data = getData(request);
            var respHtml = this.Encoding.GetString(data);
            return respHtml;
        }
        /// <summary>
        /// 通过HttpPost 方式获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="userTimeout">单位秒，超时时间</param>
        /// <returns></returns>
        protected string httpPost(string url, string postData, int userTimeout)
        {
            byte[] data = this.Encoding.GetBytes(postData);
            return httpPost(url, data, userTimeout);
        }
        private string httpPost(string url, byte[] postData, int userTimeout)
        {
            HttpWebRequest request = createRequest(url, "POST");
            request.Timeout = userTimeout;
            request.ContentType = "text/json";
            request.ContentLength = postData.Length;
            this.postData(request, postData);
            string respHtml = this.Encoding.GetString(getData(request));
            return respHtml;
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="postData"></param>
        private void postData(HttpWebRequest request, byte[] postData)
        {
            int offset = 0;
            int sendBufferSize = bufferSize;
            int remainBytes = 0;
            Stream stream = request.GetRequestStream();
            UploadEventArgs args = new UploadEventArgs();
            args.TotalBytes = postData.Length;
            while ((remainBytes = postData.Length - offset) > 0)
            {
                if (sendBufferSize > remainBytes) sendBufferSize = remainBytes;
                stream.Write(postData, offset, sendBufferSize);
                offset += sendBufferSize;
                if (this.UploadProgressChanged != null)
                {
                    args.BytesSent = offset;
                    this.UploadProgressChanged(this, args);
                }
            }
            stream.Close();
        }
        /// <summary>    
        /// 读取请求返回的数据    
        /// </summary>    
        /// <param name="request">请求对象</param>    
        /// <returns></returns>    
        private byte[] getData(HttpWebRequest request)
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            responseHeaders = response.Headers;

            DownloadEventArgs args = new DownloadEventArgs();
            if (responseHeaders[HttpResponseHeader.ContentLength] != null)
                args.TotalBytes = Convert.ToInt32(responseHeaders[HttpResponseHeader.ContentLength]);

            MemoryStream ms = new MemoryStream();
            int count = 0;
            byte[] buf = new byte[bufferSize];
            while ((count = stream.Read(buf, 0, buf.Length)) > 0)
            {
                ms.Write(buf, 0, count);
                if (this.DownloadProgressChanged != null)
                {
                    args.BytesReceived += count;
                    args.ReceivedData = new byte[count];
                    Array.Copy(buf, args.ReceivedData, count);
                    this.DownloadProgressChanged(this, args);
                }
            }
            stream.Close();
            //解压    
            if (ResponseHeaders[HttpResponseHeader.ContentEncoding] != null)
            {
                MemoryStream msTemp = new MemoryStream();
                count = 0;
                buf = new byte[100];
                switch (ResponseHeaders[HttpResponseHeader.ContentEncoding].ToLower())
                {
                    case "gzip":
                        GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress);
                        while ((count = gzip.Read(buf, 0, buf.Length)) > 0)
                        {
                            msTemp.Write(buf, 0, count);
                        }
                        return msTemp.ToArray();
                    case "deflate":
                        DeflateStream deflate = new DeflateStream(ms, CompressionMode.Decompress);
                        while ((count = deflate.Read(buf, 0, buf.Length)) > 0)
                        {
                            msTemp.Write(buf, 0, count);
                        }
                        return msTemp.ToArray();
                    default:
                        break;
                }
            }
            return ms.ToArray();
        }
        private bool checkValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        #endregion
    }

}