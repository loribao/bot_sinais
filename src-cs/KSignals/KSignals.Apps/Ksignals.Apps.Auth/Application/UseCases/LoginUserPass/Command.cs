using KSignals.Domain.Shared.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksignals.Apps.Auth.Application.UseCases.LoginUserPass
{
    public class Command : ICommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
