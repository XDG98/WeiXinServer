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
            #region ����������
            services.AddControllersWithViews(option => option.Filters.Add<ActionFilter>()).AddNewtonsoftJson();
            #endregion

            #region ע��Swagger������������һ���Ͷ��Swagger �ĵ�
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
                        Name = "С����",
                        Email = string.Empty,
                        Url = new Uri(AppSettings.GetAppSettings("DefaultUrl"))
                    },
                    License = new OpenApiLicense
                    {
                        Name = "���֤����",
                        Url = new Uri(AppSettings.GetAppSettings("DefaultUrl"))
                    }
                });
                // Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, true);
            });
            #endregion

            #region ����ʱ���ʽ
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

                #region ��������������Swagger���������� => Properties => launchSettings.json => profiles => environmentVariables => ASPNETCORE_ENVIRONMENT
                //�����м����������Swagger��ΪJSON�ս��
                app.UseSwagger();
                //�����м�������swagger-ui��ָ��Swagger JSON�ս��
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
