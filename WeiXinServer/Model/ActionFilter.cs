using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WeiXinServer;

namespace KJJOA.Model
{
    /// <summary>
    /// 方法过滤器
    /// </summary>
    public class ActionFilter : IActionFilter
    {
        /// <summary>
        /// 日志服务
        /// </summary>
        private readonly ILogger<ActionFilter> _logger;
        public ActionFilter(IConfiguration configuration, ILogger<ActionFilter> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// 方法执行后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //如果发出异常且未处理
            if (context.Exception != null && !context.ExceptionHandled)
            {
                //记录日志
                this.WriteLog(context.HttpContext, context.Exception);
                //返回错误消息
                context.Result = new JsonResult(new ServerErrorResult("系统发生异常，请联系管理员！" + context.Exception.Message));
                //将异常标记为已处理
                context.ExceptionHandled = true;
            }
            return;
        }

        /// <summary>
        /// 方法执行中
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //记录日志
            this.WriteLog(context.HttpContext);

            //请求路径
            PathString path = context.HttpContext.Request.Path;
            //请求方法
            string method = context.HttpContext.Request.Method.ToLower();
            // 用 / 分割请求路径
            string[] str = path.Value.Split("/");
            foreach (var item in str)
            {
                if (string.IsNullOrEmpty(item)) continue;
                var lower = item.ToLower();
                //登录，注册不需要token
                if ("loginin".Equals(lower) || "download".Equals(lower) || "initializereset".Equals(lower) || "postsms".Equals(lower))
                {
                    return;
                }
            }

            #region 判断登陆用户
            //DataResult dataResult;

            ////定义请求头部token
            //string token = null;
            ////获取请求中头部携带的token信息
            //token = context.HttpContext.Request.Headers["token"];
            ////获取线程安全字典保存的信息
            //ConcurrentDictionary<string, string> loginDic = ConCache.GetConnStr();
            ////Headers 中 token 不为空(非登陆请求都需要验证登录时派发的 token)
            //if (string.IsNullOrEmpty(token))
            //{
            //    dataResult = new DataResult() { Info = "请提供认证信息", ErrorCode = HttpStatusCode.Unauthorized };
            //    context.HttpContext.Response.StatusCode = HttpStatusCode.Unauthorized;
            //    context.Result = new JsonResult(dataResult);
            //}
            //else
            //{
            //    if (!loginDic.ContainsKey(token))
            //    {
            //        dataResult = new DataResult() { Info = "认证过期,请重新提供认证信息！", ErrorCode = HttpStatusCode.Unauthorized };
            //        context.HttpContext.Response.StatusCode = HttpStatusCode.Unauthorized;
            //        context.Result = new JsonResult(dataResult);
            //        return;
            //    }
            //}
            #endregion

            return;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        private void WriteLog(HttpContext context, Exception exception = null)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"------------Log------------");
            stringBuilder.Append($"\n      RequestTime：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}");
            stringBuilder.Append($"\n      RequestMethod：{context.Request.Method}");
            stringBuilder.Append($"\n      RequestPath：{context.Request.Path}");
            //string requestBody = this.GetPostJson(context);
            //if (!string.IsNullOrEmpty(requestBody)) stringBuilder.Append($"\n      RequestBody：{requestBody}");
            stringBuilder.Append($"\n      RequestIPv4：{context.Connection.RemoteIpAddress.MapToIPv4()}");

            Console.WriteLine("\n");
            if (exception != null)
            {
                stringBuilder.Append($"\n      Exception：{exception.Message}");
                this._logger.LogError($"{stringBuilder.ToString()}");
            }
            else
            {
                this._logger.LogInformation($"{stringBuilder.ToString()}");
            }
        }

        #region 获取请求参数,并且使用异步
        /// <summary>
        /// 获取请求参数,并且使用异步
        /// </summary>
        /// <returns></returns>
        public string GetPostJson(HttpContext context)
        {
            string requestBody = String.Empty;
            long? contentLength = context.Request.ContentLength;
            if (contentLength != null && contentLength > 0)
            {
                using (StreamReader streamReader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    requestBody = streamReader.ReadToEndAsync().Result;
                }
            }
            
            return requestBody;
        }
        #endregion

    }
}
