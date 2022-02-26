using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using WeiXinServer.Helper;

namespace WeiXinServer.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    [ApiController]
    [Route("Home")]
    public class HomeController : Controller
    {
        #region 首页
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("Index")]
        public IActionResult Index()
        {
            ViewData["QrCodeDic"] = Program.qrCodeDic;

            return View();
        }
        #endregion

        #region Swagger接口文档
        /// <summary>
        /// Swagger接口文档
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("Swagger")]
        public IActionResult Swagger()
        {
            string localIP = GetLocalIP();
            //Console.WriteLine($"内网(局域网)IP：{localIP}");

            string publicIP = GetPublicIP();
            //Console.WriteLine($"外网(公网)IP：{publicIP}");

            //项目端口
            int port = Request.HttpContext.Connection.LocalPort;
            //Console.WriteLine($"项目端口：{port}");
            
            string url = $"http://{publicIP}:{port}/swagger/index.html";

            return Redirect(url);
        }
        #endregion

        #region 获取内网(局域网)IP
        /// <summary>
        /// 获取内网(局域网)IP
        /// </summary>
        /// <returns></returns>
        private string GetLocalIP()
        {
            string localIP = Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString();
            //string localIP = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();

            return localIP;
        }
        #endregion

        #region 获取外网(公网)IP
        /// <summary>
        /// 获取外网(公网)IP
        /// </summary>
        /// <returns></returns>
        private string GetPublicIP()
        {
            try
            {
                string publicIP = GetLocalIP();
                RespondResult respondResult = HttpRequest.Get("http://www.ipip5.com/");
                if (respondResult.ErrorCode == HttpStatusCode.OK && respondResult.Data != null)
                {
                    string str = respondResult.Data.ToString();
                    int first = str.IndexOf("<span class=\"c-ip\">") + 19;
                    int last = str.IndexOf("</span>", first);
                    string ip = str.Substring(first, last - first);
                    publicIP = IPAddress.Parse(ip).ToString();
                }

                return publicIP;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"出错了，{ex.Message}。获取失败");
                return "";
            }
        }
        #endregion

    }
}
