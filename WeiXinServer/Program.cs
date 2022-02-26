using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WeiXinServer.Model.AccountModel;

namespace WeiXinServer
{
    public class Program
    {
        public static Dictionary<string, CreateQRCodeRespond> qrCodeDic;

        public static void Main(string[] args)
        {
            qrCodeDic = new Dictionary<string, CreateQRCodeRespond>();

            CreateHostBuilder(args).Build().Run();
            //CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string urls = AppSettings.GetAppSettings("urls");
            urls = String.IsNullOrEmpty(urls) ? "http://*:5000" : urls;
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls(urls);
                });
        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        //{
        //    string urls = AppSettings.GetAppSettings("urls");

        //    return WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .UseUrls(urls);
        //}

    }
}
