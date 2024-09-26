using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLittleToolsFairy.WebCore.CorsExtend
{
    public static class CorsExtensions
    {
        /// <summary>
        /// 配置Cors設定
        /// </summary>
        /// <param name="service"></param>
        public static void AddCorsExt(this IServiceCollection service)
        {
            service.AddCors(options =>
            {
                // allcors 是替這組配置取的名稱
                options.AddPolicy("allcors", corsBuilder =>
                {
                    corsBuilder.AllowAnyHeader()
                               .AllowAnyOrigin()
                               .AllowAnyMethod();
                });
            });
        }

        /// <summary>
        /// 啟用Cors中介
        /// </summary>
        /// <param name="app"></param>
        public static void UseCorsExt(this WebApplication app)
        {
            app.UseCors("allcors");
        }
    }
}