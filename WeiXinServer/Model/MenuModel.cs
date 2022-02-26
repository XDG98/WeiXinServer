using System.Collections.Generic;

namespace WeiXinServer.Model
{
    public class MenuModel
    {
        #region 创建自定义菜单请求参数
        /// <summary>
        /// 二级菜单数组，个数应为1~5个
        /// </summary>
        public class Sub_buttonItem
        {
            /// <summary>
            /// 菜单的响应动作类型，view表示网页类型，click表示点击类型，miniprogram表示小程序类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 菜单标题，不超过16个字节，子菜单不超过60个字节
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 菜单KEY值，用于消息接口推送，不超过128字节
            /// click等点击类型必须
            /// </summary>
            public string key { get; set; }
            /// <summary>
            /// 网页 链接，用户点击菜单可打开链接，不超过1024字节。 type为miniprogram时，不支持小程序的老版本客户端将打开本url。
            /// view、miniprogram类型必须
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 调用新增永久素材接口返回的合法media_id
            /// media_id类型和view_limited类型必须
            /// </summary>
            public string media_id { get; set; }
            /// <summary>
            /// 小程序的appid（仅认证公众号可配置）
            /// miniprogram类型必须
            /// </summary>
            public string appid { get; set; }
            /// <summary>
            /// 小程序的页面路径
            /// miniprogram类型必须 
            /// </summary>
            public string pagepath { get; set; }
            /// <summary>
            /// 发布后获得的合法 article_id
            /// article_id类型和article_view_limited类型必须 
            /// </summary>
            public string article_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> sub_button { get; set; } //= new List<string>();
        }
        /// <summary>
        /// 一级菜单数组，个数应为1~3个
        /// </summary>
        public class ButtonItem
        {
            /// <summary>
            /// 菜单标题，不超过16个字节，子菜单不超过60个字节
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 二级菜单数组，个数应为1~5个
            /// </summary>
            public List<Sub_buttonItem> sub_button { get; set; }
        }
        /// <summary>
        /// 创建自定义菜单请求参数
        /// </summary>
        public class CreateMenuRequest
        {
            /// <summary>
            /// 一级菜单数组，个数应为1~3个
            /// </summary>
            public List<ButtonItem> button { get; set; }
            /// <summary>
            /// 菜单匹配规则（个性化菜单）
            /// </summary>
            //public Matchrule matchrule { get; set; }
        }
        public class Matchrule
        {
            /// <summary>
            /// 用户标签的id，可通过用户标签管理接口获取
            /// </summary>
            public string tag_id { get; set; }
            /// <summary>
            /// 性别：男（1）女（2），不填则不做匹配
            /// 已废除
            /// </summary>
            public string sex { get; set; }
            /// <summary>
            /// 客户端版本，当前只具体到系统型号：IOS(1), Android(2),Others(3)，不填则不做匹配
            /// </summary>
            public string client_platform_type { get; set; }
            /// <summary>
            /// 国家信息，是用户在微信中设置的地区，具体请参考地区信息表
            /// 已废除
            /// </summary>
            public string country { get; set; }
            /// <summary>
            /// 省份信息，是用户在微信中设置的地区，具体请参考地区信息表
            /// 已废除
            /// </summary>
            public string province { get; set; }
            /// <summary>
            /// 城市信息，是用户在微信中设置的地区，具体请参考地区信息表
            /// 已废除
            /// </summary>
            public string city { get; set; }
            /// <summary>
            /// 语言信息，是用户在微信中设置的语言，具体请参考语言表：
            /// 1、简体中文 "zh_CN" 2、繁体中文TW "zh_TW" 3、繁体中文HK "zh_HK" 4、英文 "en"
            /// 5、印尼 "id" 6、马来 "ms" 7、西班牙 "es" 8、韩国 "ko" 9、意大利 "it"
            /// 10、日本 "ja" 11、波兰 "pl" 12、葡萄牙 "pt" 13、俄国 "ru" 14、泰文 "th"
            /// 15、越南 "vi" 16、阿拉伯语 "ar" 17、北印度 "hi" 18、希伯来 "he" 19、土耳其 "tr"
            /// 20、德语 "de" 21、法语 "fr"
            /// 已废除
            /// </summary>
            public string language { get; set; }
        }
        #endregion

        #region 获取自定义菜单配置响应结果
        //public class ButtonItem
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string type { get; set; }
        //    /// <summary>
        //    /// 今日歌曲
        //    /// </summary>
        //    public string name { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string key { get; set; }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public List<string> sub_button { get; set; }
        //}
        /// <summary>
        /// 默认菜单
        /// </summary>
        public class Menu
        {
            /// <summary>
            /// 一级菜单数组，个数应为1~3个
            /// </summary>
            public List<ButtonItem> button { get; set; }
        }
        /// <summary>
        /// 获取自定义菜单配置响应结果
        /// </summary>
        public class GetMenuRespond
        {
            /// <summary>
            /// 默认菜单
            /// </summary>
            public Menu menu { get; set; }
        }

        #endregion
    }
}
