using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CHENBI.CMQ {
    /// <summary>
    /// CMQ 签名工具
    /// </summary>
    public class CMQTool {
        /// <summary>
        /// Url 签名生成
        /// </summary>
        /// <param name="src"></param>
        /// <param name="key"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string Sign(string src, string key, string method = "sha1") {
            if (method == "sha1") {
                byte[] signByteArrary = HmacSha1Sign(src, key);
                return Convert.ToBase64String(signByteArrary);
            } else {
                byte[] signByteArrary = HmacSHA256Sign(src, key);
                return Convert.ToBase64String(signByteArrary);
            }
        }

        #region 常用函数
        /// <summary>
        /// UnixTime时间戳
        /// </summary>
        /// <param name="expired">有效期（单位：秒）</param>
        /// <returns></returns>
        public static string UnixTime(double expired = 0) {
            var time = (DateTime.Now.AddSeconds(expired).ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return time.ToString();
        }
        /// <summary>
        /// 字节数组合并
        /// </summary>
        /// <param name="byte1"></param>
        /// <param name="byte2"></param>
        /// <returns></returns>
        public static byte[] JoinByteArr(byte[] byte1, byte[] byte2) {
            byte[] full = new byte[byte1.Length + byte2.Length];
            Stream s = new MemoryStream();
            s.Write(byte1, 0, byte1.Length);
            s.Write(byte2, 0, byte2.Length);
            s.Position = 0;
            int r = s.Read(full, 0, full.Length);
            if (r > 0) {
                return full;
            }
            throw new Exception("读取错误!");
        }
        /// <summary>
        /// HMAC-SHA1 算法签名
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] HmacSha1Sign(string str, string key) {
            byte[] keyBytes = StrToByteArr(key);
            HMACSHA1 hmac = new HMACSHA1(keyBytes);
            byte[] inputBytes = StrToByteArr(str);
            return hmac.ComputeHash(inputBytes);
        }
        /// <summary>
        /// HmacSHA256 算法签名
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] HmacSHA256Sign(string str, string key) {
            byte[] keyBytes = StrToByteArr(key);
            HMACSHA256 hmac = new HMACSHA256(keyBytes);            
            byte[] inputBytes = StrToByteArr(str);
            return hmac.ComputeHash(inputBytes);
        }
        /// <summary>
        /// 字符串转字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StrToByteArr(string str) {
            return Encoding.UTF8.GetBytes(str);
        }
        /// <summary>
        /// 字节数组转字符串
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static string ByteArrToStr(byte[] byteArray) {
            return Encoding.UTF8.GetString(byteArray);
        }
        #endregion

    }

}