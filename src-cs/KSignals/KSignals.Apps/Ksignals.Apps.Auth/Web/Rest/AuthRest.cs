using Ksignals.Apps.Auth.Core.Interfaces.IApplication.IUseCases;
using KSignals.Domain.Shared.ValueObjects.Results;
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
        private record AuthRequest(string user, string pass);
        public static WebApplication AuthController(this WebApplication app)
        {
           
            app.MapPost("/auth/login/user", async (ILoginUserPassHandler handler,AuthRequest request) =>
            {
                var command = new Application.UseCases.LoginUserPass.Command
                {
                    Username = request.user,
                    Password = request.pass
                };
                var _response = await handler.HandleAsync(command);

                if (_response is Application.UseCases.LoginUserPass.Response { IsSuccess: true, Value: var valor })
                {
                    return Results.Ok($"token {valor}");
                }
                else if (_response is Application.UseCases.LoginUserPass.Response { IsSuccess: false, Error: var error })
                {
                    return Results.BadRequest(error);
                }
                else
                {
                    return Results.BadRequest("Unknown error");
                }
            })
               .WithName("AuthModule")
               .WithTags("Auth");
            return app;
        }
    }
}
