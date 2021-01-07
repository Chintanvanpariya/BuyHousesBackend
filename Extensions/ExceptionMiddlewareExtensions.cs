using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // this thing returns all exceptions occuring in api .
            }
            else  // production exception hadling done globally( no need for try catch moreover)
            {

                app.UseExceptionHandler(
                           options => {
                               options.Run(
                                   async context =>
                                   {
                                       context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                       var ex = context.Features.Get<IExceptionHandlerFeature>();
                                       if (ex != null)
                                       {
                                           await context.Response.WriteAsync(ex.Error.Message);
                                       }

                                   }
                                   );
                           }
                       );

            }

        }

    }
}