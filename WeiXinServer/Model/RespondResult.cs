using System.Net;

namespace WeiXinServer
{
    #region 响应结构
    /// <summary>
    /// 响应结构
    /// </summary>
    public class RespondResult
    {
        /// <summary>
        /// 响应状态码
        /// </summary>
        public HttpStatusCode ErrorCode { get; set; }
        /// <summary>
        /// 响应消息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 响应数据
        /// </summary>
        public object Data { get; set; }
    }
    #endregion

    #region 成功响应
    /// <summary>
    /// 成功响应
    /// </summary>
    public class SucceedResult : RespondResult
    {
        public SucceedResult(object obj = null)
        {
            this.ErrorCode = HttpStatusCode.OK;
            this.ErrorMsg = "OK";
            this.Data = obj;
        }
    }
    #endregion

    #region 失败响应
    /// <summary>
    /// 失败响应
    /// </summary>
    public class FailResult : RespondResult
    {
        public FailResult(string errorMsg = "操作失败！", HttpStatusCode errorCode = HttpStatusCode.BadRequest)
        {
            this.ErrorCode = errorCode;
            this.ErrorMsg = errorMsg;
            this.Data = null;
        }
    }
    #endregion

    #region 服务器错误响应
    /// <summary>
    /// 服务器错误响应
    /// </summary>
    public class ServerErrorResult : RespondResult
    {
        public ServerErrorResult(string errorMsg = "服务器错误！")
        {
            this.ErrorCode = HttpStatusCode.InternalServerError;
            this.ErrorMsg = errorMsg;
            this.Data = null;
        }
    }
    #endregion
}
