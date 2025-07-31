using Ksignals.Apps.Auth.Application.UseCases.LoginUserPass;
using KSignals.Domain.Shared.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksignals.Apps.Auth.Core.Interfaces.IApplication.IUseCases
{
    internal interface ILoginUserPassHandler : IHandler<Command, Response>
    {
    }
}
