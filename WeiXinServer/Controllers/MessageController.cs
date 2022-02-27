using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using WeiXinServer.Helper;
using static WeiXinServer.Model.MessageModel;
using static WeiXinServer.Model.UserModel;

namespace WeiXinServer.Controllers
{
    /// <summary>
    /// 消息管理
    /// </summary>
    [ApiController]
    [Route("Message")]
    public class MessageController : Controller
    {
        /// <summary>
        /// 微信公众号ApiUrl
        /// </summary>
        private static string weiXinApiUrl;
        /// <summary>
        /// 微信公众号AccessToken
        /// </summary>
        private static string access_token;
        public MessageController()
        {
            weiXinApiUrl = AppSettings.GetAppSettings("WeiXinApiUrl");
            access_token = AppSettings.GetAppSettings("AccessToken");
        }

        #region 发送客服消息
        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="data">发送客服消息请求参数</param>
        /// <returns></returns>
        [HttpPost, Route("SendCustomMessage")]
        public RespondResult SendCustomMessage([FromBody] SendMessageRequest data)
        {
            if (string.IsNullOrEmpty(data.touser)) return new FailResult("用户ID不能为空");
            if (string.IsNullOrWhiteSpace(data.text.content)) return new FailResult("消息内容不能为空");

            string url = $"{weiXinApiUrl}/message/custom/send?access_token={access_token}";

            #region 添加参数
            SendMessageRequest sendMessageRequest = new SendMessageRequest()
            {
                touser = data.touser,
                msgtype = "text",
                //msgtype = data.msgtype,
                text = new MessageRequest()
                {
                    content = data.text.content,
                }
            };
            #endregion

            string parameterString = JsonConvert.SerializeObject(sendMessageRequest);
            RespondResult respondResult = HttpRequest.Post(url, parameterString);

            return respondResult;
        }
        #endregion

        #region 发送模板消息
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("SendTemplateMessage")]
        public RespondResult SendTemplateMessage([FromBody] SendTemplateMessageRequest data)
        {
            if (string.IsNullOrEmpty(data.touser)) return new FailResult("用户ID不能为空");

            string url = $"{weiXinApiUrl}/message/template/send?access_token={access_token}";

            #region 获取用户信息
            RespondResult respondResult1 = new UserController().GetUserInfo(data.touser);
            if (respondResult1.ErrorCode == HttpStatusCode.BadRequest) return respondResult1;
            GetFocusUserInfoRespond getFocusUserInfoRespond = respondResult1.Data as GetFocusUserInfoRespond;
            #endregion
            #region 添加参数
            SendTemplateMessageRequest sendTemplateMessageRequest = new SendTemplateMessageRequest()
            {
                touser = getFocusUserInfoRespond.openid,
                template_id = "DKp7xjhg4ls0Lu5RC8UvCYr9YDPerj9qV0icZaVCTdg",
                //url = "http://weixin.qq.com/download",
                url = AppSettings.GetAppSettings("DefaultUrl"),
                topcolor = "#FF0000",
                data = new TemplateMessageRequest()
                {
                    User = new User() { value = $"{getFocusUserInfoRespond.nickname}先生", color = "#173177", },
                    Date = new Date() { value = "06月07日 19时24分", color = "#173177", },
                    CardNumber = new CardNumber() { value = "0426", color = "#173177", },
                    DealType = new DealType() { value = "消费", color = "#173177", },
                    Money = new Money() { value = "人民币260.00元", color = "#173177", },
                    DeadTime = new DeadTime() { value = "06月07日19时24分", color = "#173177", },
                    Left = new Left() { value = "6504.09", color = "#173177", },
                }
            };
            #endregion

            string parameterString = JsonConvert.SerializeObject(sendTemplateMessageRequest);
            RespondResult respondResult = HttpRequest.Post(url, parameterString);

            return respondResult;
        }
        #endregion
    }
}
