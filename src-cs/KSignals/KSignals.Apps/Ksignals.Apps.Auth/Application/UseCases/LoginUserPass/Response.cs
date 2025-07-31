using KSignals.Domain.Shared.UseCases;
using KSignals.Domain.Shared.ValueObjects.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksignals.Apps.Auth.Application.UseCases.LoginUserPass
{
    public record Response : ResponseBase<string>
    {
        public Response(string Value, bool IsSuccess, string? Error) : base(Value, IsSuccess, Error)
        {
        }
    }
}
