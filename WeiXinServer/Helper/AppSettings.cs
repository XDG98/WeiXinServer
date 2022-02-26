using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace WeiXinServer
{
    /// <summary>
    /// 配置文件管理
    /// </summary>
    public class AppSettings
    {
        static IConfiguration Configuration { get; set; }
        static AppSettings()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Add(new JsonConfigurationSource
                {
                    Path = "appsettings.json",
                    //ReloadOnChange = true; 当appsettings.json被修改时重新加载
                    ReloadOnChange = true
                })
                .Build();
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="sections">配置</param>
        /// <returns></returns>
        public static string GetAppSettings(params string[] sections)
        {
            try
            {
                var val = string.Empty;
                for (int i = 0; i < sections.Length; i++)
                {
                    val += sections[i] + ":";
                }

                return Configuration[val.TrimEnd(':')];
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="sections">配置</param>
        /// <returns></returns>
        public static void SaveAppSettings(string key, string value)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

                StreamReader streamReader = File.OpenText(filePath);
                JsonTextReader jsonTextReader = new JsonTextReader(streamReader);
                JObject jsonObject = (JObject)JToken.ReadFrom(jsonTextReader);

                jsonObject[key] = value;

                streamReader.Close();
                string contents = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                File.WriteAllText(filePath, contents);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
