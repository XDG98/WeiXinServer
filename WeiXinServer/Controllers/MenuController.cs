using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WeiXinServer.Helper;
using static WeiXinServer.Model.MenuModel;

namespace WeiXinServer.Controllers
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [ApiController]
    [Route("Menu")]
    public class MenuController : Controller
    {
        /// <summary>
        /// 微信公众号ApiUrl
        /// </summary>
        private static string weiXinApiUrl;
        /// <summary>
        /// 微信公众号AccessToken
        /// </summary>
        private static string access_token;
        public MenuController()
        {
            weiXinApiUrl = AppSettings.GetAppSettings("WeiXinApiUrl");
            access_token = AppSettings.GetAppSettings("AccessToken");
        }

        #region 创建自定义菜单
        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("CreateMenu")]
        public RespondResult CreateMenu()
        {
            string url = $"{weiXinApiUrl}/menu/create?access_token={access_token}";

            #region 添加参数
            CreateMenuRequest createMenuRequest = new CreateMenuRequest()
            {
                button = new List<ButtonItem>()
                    {
                        #region 菜单一
                        new ButtonItem()
                        {
                            name = "菜单一",
                            sub_button = new List<Sub_buttonItem>()
                            {
                                new Sub_buttonItem()
                                {
                                    type = "view",
                                    name = "跳转网页",
                                    url = AppSettings.GetAppSettings("DefaultUrl"),
                                },
                                new Sub_buttonItem()
                                {
                                    type = "click",
                                    name = "赞咱我们一下",
                                    key = "V1001_GOOD"
                                },
                                new Sub_buttonItem()
                                {
                                    type = "scancode_waitmsg",
                                    name = "扫码带提示",
                                    key = "rselfmenu_0_0"
                                },
                                new Sub_buttonItem()
                                {
                                    type = "scancode_push",
                                    name = "扫码推事件",
                                    key = "rselfmenu_0_1"
                                }
                            }
                        },
                        #endregion
                        #region 菜单二
                        new ButtonItem()
                        {
                            name = "菜单二",
                            sub_button = new List<Sub_buttonItem>()
                            {
                                new Sub_buttonItem()
                                {
                                    type = "pic_sysphoto",
                                    name = "系统拍照发图",
                                    key = "rselfmenu_1_0"
                                },
                                new Sub_buttonItem()
                                {
                                    type = "pic_photo_or_album",
                                    name = "拍照或者相册发图",
                                    key = "rselfmenu_1_1"
                                },
                                new Sub_buttonItem()
                                {
                                    type = "pic_weixin",
                                    name = "微信相册发图",
                                    key = "rselfmenu_1_2"
                                }
                            }
                        },
                        #endregion
                        #region 菜单三
                        new ButtonItem()
                        {
                            name = "菜单三",
                            sub_button = new List<Sub_buttonItem>()
                            {
                                new Sub_buttonItem()
                                {
                                    type = "location_select",
                                    name = "发送位置",
                                    key = "rselfmenu_2_0"
                                },
                                //new Sub_buttonItem()
                                //{
                                //    type = "media_id",
                                //    name = "图片",
                                //    media_id = "MEDIA_ID1"
                                //},
                                //new Sub_buttonItem()
                                //{
                                //    type = "view_limited",
                                //    name = "图文消息",
                                //    media_id = "MEDIA_ID2"
                                //},
                                //new Sub_buttonItem()
                                //{
                                //    type = "article_id",
                                //    name = "发布后的图文消息",
                                //    article_id = "ARTICLE_ID1"
                                //},
                                //new Sub_buttonItem()
                                //{
                                //    type = "article_view_limited",
                                //    name = "发布后的图文消息",
                                //    article_id = "ARTICLE_ID2"
                                //}
                            }
                        }
                        #endregion
                    }
            };
            #endregion

            string parameterString = JsonConvert.SerializeObject(createMenuRequest);
            RespondResult respondResult = HttpRequest.Post(url, parameterString);

            return new SucceedResult(respondResult);
        }
        #endregion

        #region 获取自定义菜单配置
        /// <summary>
        /// 获取自定义菜单配置
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetMenu")]
        public RespondResult GetMenu()
        {
            string url = $"{weiXinApiUrl}/menu/get";

            #region 添加参数
            Dictionary<string, string> parameterDic = new Dictionary<string, string>();
            parameterDic.Add("access_token", access_token);
            #endregion

            RespondResult respondResult = HttpRequest.Get(url, parameterDic);
            GetMenuRespond getMenuRespond = JsonConvert.DeserializeObject<GetMenuRespond>(respondResult.Data.ToString());

            return new SucceedResult(getMenuRespond);
        }
        #endregion
    }
}
