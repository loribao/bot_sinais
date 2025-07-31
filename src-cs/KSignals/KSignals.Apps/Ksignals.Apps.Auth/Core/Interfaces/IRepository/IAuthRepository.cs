using KSignals.Domain.Shared.ValueObjects.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksignals.Apps.Auth.Core.Interfaces.IRepository
{
    public interface IAuthRepository
    {
        Task<bool> AuthenticateUserAsync(string username, Password password);
    }
}
