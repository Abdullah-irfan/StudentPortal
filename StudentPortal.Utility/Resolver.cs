using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StudentPortal.Core;
using StudentPortal.Repository;
using StudentPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortal.Utility
{
  public class Resolver
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            #region Context accesor
            // this is for accessing the http context by interface in view level
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region Services

            services.AddScoped<IStudentPortalServices, StudentPortalServices>();

            #endregion

            #region Repository

            services.AddScoped<IStudentPortalRepository, StudentPortalRepository>();

            #endregion

        }
    }
}
