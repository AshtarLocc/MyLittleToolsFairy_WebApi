using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLittleToolsFairy.webcore.SwaggerExtend
{
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Swagger的設置項
        /// </summary>
        /// <param name="service"></param>
        public static void AddSwaggerExt(this IServiceCollection service)
        {
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen(option =>
            {
                foreach (var version in typeof(ApiVersions).GetEnumNames())
                {
                    option.SwaggerDoc(version, new OpenApiInfo()
                    {
                        Title = "My Lilttle Tools Fairy Site - Api Document",
                        Version = version,
                        Description = $"api {version}"
                    });
                }

                // xml指定路徑
                var file = Path.Combine(AppContext.BaseDirectory, $"{AppDomain.CurrentDomain.FriendlyName}.xml");
                // true:顯示controller的註解
                option.IncludeXmlComments(file, true);
                // 排序Action的名稱
                option.OrderActionsBy(o => o.RelativePath);

                #region 啟用 Jwt Token 授權

                {
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "input Token pls",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference=new OpenApiReference()
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
                }

                #endregion 啟用 Jwt Token 授權
            });
        }

        /// <summary>
        /// 使用Swagger中介
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerExt(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                foreach (var version in typeof(ApiVersions).GetEnumNames())
                {
                    option.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"My Lilttle Tools Fairy Site - Api Document {version}");
                }
            });
        }
    }
}