using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using WeiXinServer.Helper;

namespace WeiXinServer.Controllers
{
    /// <summary>
    /// 微信公众号服务器
    /// </summary>
    [ApiController]
    [Route("WeiXin")]
    public class WeiXinController : Controller
    {
        #region 微信公众号服务器对接
        /// <summary>
        /// 微信公众号服务器对接
        /// </summary>
        /// <returns></returns>
        [HttpGet,HttpPost, Route("Main")]
        public string Main()
        {
            string token = AppSettings.GetAppSettings("Token");
            string signature = HttpContext.Request.Query["signature"];
            string timestamp = HttpContext.Request.Query["timestamp"];
            string nonce = HttpContext.Request.Query["nonce"];
            string echostr = HttpContext.Request.Query["echostr"];

            #region 加密消息参数
            string msg_signature = HttpContext.Request.Query["msg_signature"];
            string encrypt_type = HttpContext.Request.Query["encrypt_type"];
            #endregion

            if (HttpContext.Request.Method == HttpMethod.Get.Method)
            {
                #region 验证签名
                List<string> tmpArr = new List<string>() { token, timestamp, nonce };
                tmpArr.Sort();
                string tmpStr = string.Join("", tmpArr);
                string sha1TmpStr = EncryptAndDecrypt.SHA1Encrypt(tmpStr);

                if (sha1TmpStr == signature)
                {
                    //HttpContext.Response.WriteAsync(echostr);
                    return echostr;
                }
                else
                {
                    //HttpContext.Response.WriteAsync("validation failed");
                    return "validation failed";
                }
                #endregion
            }
            else if (HttpContext.Request.Method == HttpMethod.Post.Method)
            {
                #region 接收消息
                string receiveXmlData = String.Empty;
                using (StreamReader streamReader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
                {
                    receiveXmlData = streamReader.ReadToEndAsync().Result;
                }
                Console.WriteLine($"\n接收XML：\n{receiveXmlData}");
                #endregion

                #region 解密消息
                if (encrypt_type == "aes")
                {
                    string decryptMsg = "";
                    string EncodingAESKey = AppSettings.GetAppSettings("EncodingAESKey");
                    EncryptAndDecrypt.WXBizMsgCrypt(token, EncodingAESKey, AppSettings.GetAppSettings("AppID"));
                    int code = EncryptAndDecrypt.DecryptMsg(msg_signature, timestamp, nonce, receiveXmlData, ref decryptMsg);
                    if (code == 0)
                    {
                        receiveXmlData = decryptMsg;
                    }
                    Console.WriteLine($"\n解密结果：{code}\n解密后XML：\n{receiveXmlData}");
                }
                #endregion

                #region 格式化xml数据
                System.Xml.Linq.XElement xml = System.Xml.Linq.XElement.Parse(receiveXmlData);

                //消息接收方
                string ToUserName = xml.Element("ToUserName").Value.Trim();
                //消息发送方
                string FromUserName = xml.Element("FromUserName").Value.Trim();
                //时间戳
                long CreateTime = DateTime.Now.Ticks;
                //消息类型
                string MsgType = xml.Element("MsgType").Value.Trim();
                //消息内容
                string Content = xml.Element("Content").Value.Trim();
                //消息ID
                string MsgId = xml.Element("MsgId").Value.Trim();
                #endregion

                #region 发送消息
                string sendXmlData = String.Empty;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<xml>");
                stringBuilder.Append($"<ToUserName><![CDATA[{FromUserName}]]></ToUserName>");
                stringBuilder.Append($"<FromUserName><![CDATA[{ToUserName}]]></FromUserName>");
                stringBuilder.Append($"<CreateTime>{CreateTime}</CreateTime>");
                stringBuilder.Append($"<MsgType><![CDATA[{MsgType}]]></MsgType>");
                stringBuilder.Append($"<Content><![CDATA[{Content}]]></Content>");
                stringBuilder.Append($"<MsgId>{MsgId}</MsgId>");
                stringBuilder.Append("</xml>");
                sendXmlData = stringBuilder.ToString();

                Console.WriteLine($"\n发送明文XML：\n{sendXmlData}");
                #endregion

                #region 加密消息
                if (encrypt_type == "aes")
                {
                    string encryptMsg = "";
                    int code = EncryptAndDecrypt.EncryptMsg(sendXmlData, timestamp, nonce, ref encryptMsg);
                    if (code == 0)
                    {
                        sendXmlData = encryptMsg;
                    }
                    Console.WriteLine($"\n加密结果：{code}\n发送加密XML：\n{sendXmlData}");
                }
                #endregion

                //HttpContext.Response.WriteAsync(sendXmlData);
                return sendXmlData;
            }
            else
            {
                //HttpContext.Response.WriteAsync("success");
                return "success";
            }
        }
        #endregion
    }
}
