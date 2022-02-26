using KJJOA.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WeiXinServer.Model;

namespace WeiXinServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 方法过滤器
            services.AddControllersWithViews(option => option.Filters.Add<ActionFilter>()).AddNewtonsoftJson();
            #endregion

            #region 注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WeiXinServer",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = null,
                    Contact = new OpenApiContact
                    {
                        Name = "小呆瓜",
                        Email = string.Empty,
                        Url = new Uri(AppSettings.GetAppSettings("DefaultUrl"))
                    },
                    License = new OpenApiLicense
                    {
                        Name = "许可证名字",
                        Url = new Uri(AppSettings.GetAppSettings("DefaultUrl"))
                    }
                });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, true);
            });
            #endregion

            #region 处理时间格式
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss:fff";
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                #region 开发环境才启用Swagger，环境配置 => Properties => launchSettings.json => profiles => environmentVariables => ASPNETCORE_ENVIRONMENT
                //启用中间件服务生成Swagger作为JSON终结点
                app.UseSwagger();
                //启用中间件服务对swagger-ui，指定Swagger JSON终结点
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "WeiXinServer API V1");
                });
                #endregion
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
