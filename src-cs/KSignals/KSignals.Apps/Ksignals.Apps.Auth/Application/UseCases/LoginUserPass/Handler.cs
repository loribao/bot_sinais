using KSignals.Domain.Shared.UseCases;
using KSignals.Domain.Shared.ValueObjects.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksignals.Apps.Auth.Application.UseCases.LoginUserPass
{
    public class Handler : IHandler<Command, Response>
    {
        public Task<Response> HandleAsync(Command command)
        {
           return Task.FromResult(
                new Response(
                    Value: "token asdlfasçldfalsdj---alsdflçajl",
                    IsSuccess: true,
                    Error: null
                )
            );
        }
    }
}
