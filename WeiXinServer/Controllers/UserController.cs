using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WeiXinServer.Helper;
using static WeiXinServer.Model.UserModel;

namespace WeiXinServer.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [ApiController]
    [Route("User")]
    public class UserController : Controller
    {
        /// <summary>
        /// 微信公众号ApiUrl
        /// </summary>
        private static string weiXinApiUrl;
        /// <summary>
        /// 微信公众号AccessToken
        /// </summary>
        private static string access_token;
        public UserController()
        {
            weiXinApiUrl = AppSettings.GetAppSettings("WeiXinApiUrl");
            access_token = AppSettings.GetAppSettings("AccessToken");
        }

        #region 获取关注者列表
        /// <summary>
        /// 获取关注者列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetUserList")]
        public RespondResult GetUserList()
        {
            string url = $"{weiXinApiUrl}/user/get";

            #region 添加参数
            Dictionary<string, string> parameterDic = new Dictionary<string, string>();
            parameterDic.Add("access_token", access_token);
            #endregion

            RespondResult respondResult = HttpRequest.Get(url, parameterDic);
            GetFocusUserListRespond getFocusUserListRespond = JsonConvert.DeserializeObject<GetFocusUserListRespond>(respondResult.Data.ToString());
            if (getFocusUserListRespond == null || getFocusUserListRespond.data == null) return respondResult;

            return new SucceedResult(getFocusUserListRespond);
        }
        #endregion

        #region 获取用户基本信息
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="openid">用户的标识</param>
        /// <returns></returns>
        [HttpGet, Route("GetUserInfo")]
        public RespondResult GetUserInfo(string openid)
        {
            string url = $"{weiXinApiUrl}/user/info";

            #region 添加参数
            openid = string.IsNullOrEmpty(openid) ? "ovMAV63LUNuLCvoFThakYKNo1DOI" : openid;
            Dictionary<string, string> parameterDic = new Dictionary<string, string>();
            parameterDic.Add("access_token", access_token);
            parameterDic.Add("openid", openid);
            #endregion

            RespondResult respondResult = HttpRequest.Get(url, parameterDic);
            GetFocusUserInfoRespond getFocusUserInfoRespond = JsonConvert.DeserializeObject<GetFocusUserInfoRespond>(respondResult.Data.ToString());
            if (string.IsNullOrEmpty(getFocusUserInfoRespond.openid)) return new FailResult("用户不存在");

            return new SucceedResult(getFocusUserInfoRespond);
        }
        #endregion
    }
}
