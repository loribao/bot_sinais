using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksignals.Apps.Auth.Web.Rest
{
    public static class AuthRest
    {
        public static WebApplication AuthController(this WebApplication app)
        {
             app.MapGet("/auth", () => Results.Ok("Auth Module is running"))
                .WithName("AuthModule")
                .WithTags("Auth");



            return app;
        }
    }
}
