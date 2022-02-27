using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static WeiXinServer.Model.AccountModel;

namespace WeiXinServer.Controllers
{
    /// <summary>
    /// 账户管理
    /// </summary>
    [ApiController]
    [Route("Account")]
    public class AccountController : Controller
    {
        /// <summary>
        /// 微信公众号ApiUrl
        /// </summary>
        private static string weiXinApiUrl;
        /// <summary>
        /// 微信公众号MpUrl
        /// </summary>
        private static string weiXinMpUrl;
        /// <summary>
        /// 微信公众号AppID
        /// </summary>
        private static string appID;
        /// <summary>
        /// 微信公众号AppSecret
        /// </summary>
        private static string appSecret;
        /// <summary>
        /// 微信公众号AccessToken
        /// </summary>
        private static string access_token;

        public AccountController()
        {
            weiXinApiUrl = AppSettings.GetAppSettings("WeiXinApiUrl");
            weiXinMpUrl = AppSettings.GetAppSettings("WeiXinMpUrl");
            appID = AppSettings.GetAppSettings("AppID");
            appSecret = AppSettings.GetAppSettings("AppSecret");
            access_token = AppSettings.GetAppSettings("AccessToken");
        }

        #region 获取access_token
        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAccessToken")]
        public RespondResult GetAccessToken()
        {
            string url = $"{weiXinApiUrl}/token";

            #region 添加参数
            Dictionary<string, string> parameterDic = new Dictionary<string, string>();
            parameterDic.Add("grant_type", "client_credential");
            parameterDic.Add("appid", appID);
            parameterDic.Add("secret", appSecret);
            #endregion

            RespondResult respondResult = Helper.HttpRequest.Get(url, parameterDic);
            GetAccessTokenRespond getAccessTokenRespond = JsonConvert.DeserializeObject<GetAccessTokenRespond>(respondResult.Data.ToString());
            if (getAccessTokenRespond == null || string.IsNullOrEmpty(getAccessTokenRespond.access_token)) return respondResult;

            //保存配置文件
            //SaveAppSettings("AccessToken", getAccessTokenRespond.access_token);

            return new SucceedResult(getAccessTokenRespond);
        }
        #endregion

        #region 获取域名IP
        /// <summary>
        /// 获取域名IP
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetApiDomainIP")]
        public RespondResult GetApiDomainIP()
        {
            string url = $"{weiXinApiUrl}/get_api_domain_ip";

            #region 添加参数
            Dictionary<string, string> parameterDic = new Dictionary<string, string>();
            parameterDic.Add("access_token", access_token);
            #endregion

            RespondResult respondResult = Helper.HttpRequest.Get(url, parameterDic);
            GetApiDomainIPRespond getApiDomainIPRespond = JsonConvert.DeserializeObject<GetApiDomainIPRespond>(respondResult.Data.ToString());
            if (getApiDomainIPRespond.ip_list == null || getApiDomainIPRespond.ip_list.Count == 0) return respondResult;

            return new SucceedResult(getApiDomainIPRespond);
        }
        #endregion

        #region 创建二维码ticket
        /// <summary>
        /// 创建二维码ticket
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("CreateQRCode")]
        public RespondResult CreateQRCode([FromBody] CreateQRCodeRequest data)
        {
            /*
             * 临时二维码请求说明
             * http请求方式:
             * POST URL: https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=TOKEN
             * POST数据格式：json POST数据例子：{"expire_seconds": 604800, "action_name": "QR_SCENE", "action_info": {"scene": {"scene_id": 123}}}
             * 或者也可以使用以下POST数据创建字符串形式的二维码参数：{"expire_seconds": 604800, "action_name": "QR_STR_SCENE", "action_info": {"scene": {"scene_str": "test"}}}
             * 
             * 永久二维码请求说明
             * http请求方式: 
             * POST URL: https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=TOKEN
             * POST数据格式：json POST数据例子：{"action_name": "QR_LIMIT_SCENE", "action_info": {"scene": {"scene_id": 123}}}
             * 或者也可以使用以下POST数据创建字符串形式的二维码参数： {"action_name": "QR_LIMIT_STR_SCENE", "action_info": {"scene": {"scene_str": "test"}}}
             */

            string url = $"{weiXinApiUrl}/qrcode/create?access_token={access_token}";

            #region 添加参数
            CreateQRCodeRequest createQRCodeRequest = new CreateQRCodeRequest()
            {
                expire_seconds = 604800,
                action_name = "QR_STR_SCENE",
                action_info = new Action_info()
                {
                    scene = new Scene()
                    {
                        scene_str = "test小呆瓜",
                    }
                }
            };
            #endregion
            throw new Exception("e1");
            string parameterString = JsonConvert.SerializeObject(createQRCodeRequest);
            RespondResult respondResult = Helper.HttpRequest.Post(url, parameterString);
            CreateQRCodeRespond createQRCodeRespond = JsonConvert.DeserializeObject<CreateQRCodeRespond>(respondResult.Data.ToString());
            if (string.IsNullOrEmpty(createQRCodeRespond.ticket)) return respondResult;

            createQRCodeRespond.QrCodeUrl = $"{weiXinMpUrl}/showqrcode?ticket={createQRCodeRespond.ticket}";
            createQRCodeRespond.CreateTime = DateTime.Now;
            Program.qrCodeDic.Add(createQRCodeRespond.ticket, createQRCodeRespond);

            return new SucceedResult(createQRCodeRespond);
        }
        #endregion

        #region 换取二维码
        /// <summary>
        /// 换取二维码
        /// </summary>
        /// <param name="ticket">二维码ticket</param>
        /// <returns></returns>
        [HttpGet, Route("ShowQRCode")]
        public RespondResult ShowQRCode(string ticket)
        {
            ticket = string.IsNullOrEmpty(ticket) ? "" : Program.qrCodeDic.Keys.FirstOrDefault();
            if (string.IsNullOrEmpty(ticket)) return new FailResult("请先创建二维码ticket");

            string url = $"{weiXinMpUrl}/showqrcode";

            #region 添加参数
            //Dictionary<string, string> parameterDic = new Dictionary<string, string>();
            //parameterDic.Add("ticket", ticket);
            #endregion

            //RespondResult respondResult = HttpRequest.Get(url, parameterDic);

            return new SucceedResult($"{url}?ticket={ticket}");
        }
        #endregion

        #region 获取所有二维码
        /// <summary>
        /// 获取所有二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllQRCode")]
        public RespondResult GetAllQRCode()
        {
            return new SucceedResult(Program.qrCodeDic.Values);
        }
        #endregion
    }
}
