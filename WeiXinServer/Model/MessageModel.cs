namespace WeiXinServer.Model
{
    /// <summary>
    /// 消息管理实体类
    /// </summary>
    public class MessageModel
    {
        #region 发送客服消息请求参数
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public class MessageRequest
        {
            /// <summary>
            /// 文本消息内容
            /// </summary>
            public string content { get; set; }
        }
        /// <summary>
        /// 发送客服消息请求参数
        /// </summary>
        public class SendMessageRequest
        {
            /// <summary>
            /// 普通用户openid
            /// </summary>
            public string touser { get; set; }
            /// <summary>
            /// 消息类型，文本为text，图片为image，语音为voice，视频消息为video，音乐消息为music，
            /// 图文消息（点击跳转到外链）为news，图文消息（点击跳转到图文消息页面）为mpnews，
            /// 卡券为wxcard，小程序为miniprogrampage
            /// </summary>
            public string msgtype { get; set; }
            /// <summary>
            /// 文本消息内容
            /// </summary>
            public MessageRequest text { get; set; }
        }
        #endregion

        #region 发送模板消息请求参数
        public class User
        {
            /// <summary>
            /// 黄先生
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string color { get; set; }
        }
        public class Date
        {
            /// <summary>
            /// 06月07日 19时24分
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string color { get; set; }
        }
        public class CardNumber
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string color { get; set; }
        }
        public class DealType
        {
            /// <summary>
            /// 消费
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string color { get; set; }
        }
        public class Money
        {
            /// <summary>
            /// 人民币260.00元
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string color { get; set; }
        }
        public class DeadTime
        {
            /// <summary>
            /// 06月07日19时24分
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string color { get; set; }
        }
        public class Left
        {
            /// <summary>
            /// 
            /// </summary>
            public string value { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string color { get; set; }
        }
        public class TemplateMessageRequest
        {
            /// <summary>
            /// 
            /// </summary>
            public User User { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Date Date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public CardNumber CardNumber { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public DealType DealType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Money Money { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public DeadTime DeadTime { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Left Left { get; set; }
        }
        /// <summary>
        /// 发送模板消息请求参数
        /// </summary>
        public class SendTemplateMessageRequest
        {
            /// <summary>
            /// 
            /// </summary>
            public string touser { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string template_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string topcolor { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public TemplateMessageRequest data { get; set; }
        }
        #endregion
    }
}
