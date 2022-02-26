using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WeiXinServer.Helper
{
    /// <summary>
    /// Http请求
    /// </summary>
    public class HttpRequest
    {
        #region Get请求
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="dic">请求参数定义</param>
        /// <returns></returns>
        public static RespondResult Get(string url, Dictionary<string, string> parameterDic = null)
        {
            RespondResult respondResult = new RespondResult();

            #region 添加参数
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (parameterDic != null && parameterDic.Count > 0)
            {
                int i;
                if (!builder.ToString().Contains("?"))
                {
                    i = 0;
                    builder.Append("?");
                }
                else
                {
                    i = 1;
                }
                foreach (var item in parameterDic)
                {
                    if (i > 0) builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            #endregion

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    respondResult.ErrorCode = resp.StatusCode;
                    respondResult.ErrorMsg = resp.StatusDescription;
                    respondResult.Data = JsonConvert.DeserializeObject(reader.ReadToEnd());
                }
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }

            return respondResult;
        }
        #endregion


        #region Post请求
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dic">请求参数定义</param>
        /// <returns></returns>
        public static RespondResult Post(string url, Dictionary<string, string> parameterDic)
        {
            RespondResult respondResult = new RespondResult();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            #region 添加参数
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in parameterDic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    respondResult.ErrorCode = resp.StatusCode;
                    respondResult.ErrorMsg = resp.StatusDescription;
                    respondResult.Data = JsonConvert.DeserializeObject(reader.ReadToEnd());
                }
            }
            finally
            {
                stream.Close();
            }

            return respondResult;
        }
        #endregion

        #region Post请求
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="dic">请求参数定义</param>
        /// <returns></returns>
        public static RespondResult Post(string url, string parameterString)
        {
            RespondResult respondResult = new RespondResult();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            #region 添加Post 参数
            byte[] data = Encoding.UTF8.GetBytes(parameterString);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    respondResult.ErrorCode = resp.StatusCode;
                    respondResult.ErrorMsg = resp.StatusDescription;
                    respondResult.Data = JsonConvert.DeserializeObject(reader.ReadToEnd());
                }
            }
            finally
            {
                stream.Close();
            }

            return respondResult;
        }
        #endregion
    }
}
