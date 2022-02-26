using System;
using System.Collections.Generic;

namespace WeiXinServer.Model
{
    /// <summary>
    /// 账户管理实体类
    /// </summary>
    public class AccountModel
    {
        #region 获取微信公众号access_token响应结果
        /// <summary>
        /// 获取微信公众号access_token响应结果
        /// </summary>
        public class GetAccessTokenRespond
        {
            public string access_token { get; set; } = string.Empty;
            public int expires_in { get; set; }
        }
        #endregion

        #region 获取域名IP响应结果
        /// <summary>
        /// 获取域名IP响应结果
        /// </summary>
        public class GetApiDomainIPRespond
        {
            /// <summary>
            /// 域名IP列表
            /// </summary>
            public List<string> ip_list { get; set; }
        }
        #endregion

        #region 生成带参数二维码请求参数
        /// <summary>
        /// 场景值
        /// </summary>
        public class Scene
        {
            /// <summary>
            /// 场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）
            /// </summary>
            public int scene_id { get; set; }
            /// <summary>
            /// 场景值ID（字符串形式的ID），字符串类型，长度限制为1到64
            /// </summary>
            public string scene_str { get; set; }
        }
        /// <summary>
        /// 二维码详细信息
        /// </summary>
        public class Action_info
        {
            /// <summary>
            /// 场景值
            /// </summary>
            public Scene scene { get; set; }
        }
        /// <summary>
        /// 生成带参数二维码请求参数
        /// </summary>
        public class CreateQRCodeRequest
        {
            /// <summary>
            /// 该二维码有效时间，以秒为单位。 最大不超过2592000（即30天），此字段如果不填，则默认有效期为60秒。
            /// </summary>
            public int expire_seconds { get; set; }
            /// <summary>
            /// 二维码类型，QR_SCENE为临时的整型参数值，QR_STR_SCENE为临时的字符串参数值，
            /// QR_LIMIT_SCENE为永久的整型参数值，QR_LIMIT_STR_SCENE为永久的字符串参数值
            /// </summary>
            public string action_name { get; set; }
            /// <summary>
            /// 二维码详细信息
            /// </summary>
            public Action_info action_info { get; set; }
        }
        #endregion

        #region 生成带参数二维码响应结果
        /// <summary>
        /// 生成带参数二维码响应结果
        /// </summary>
        public class CreateQRCodeRespond
        {
            /// <summary>
            /// 获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码。
            /// </summary>
            public string ticket { get; set; }
            /// <summary>
            /// 该二维码有效时间，以秒为单位。 最大不超过2592000（即30天）。
            /// </summary>
            public int expire_seconds { get; set; }
            /// <summary>
            /// 二维码图片解析后的地址，开发者可根据该地址自行生成需要的二维码图片
            /// </summary>
            public string url { get; set; }


            /// <summary>
            /// 二维码拼接地址
            /// </summary>
            public string QrCodeUrl { get; set; }
            /// <summary>
            /// 二维码生成时间
            /// </summary>
            public DateTime CreateTime { get; set; } = DateTime.MinValue;
        }
        #endregion
    }
}
