using Ksignals.Apps.Auth.Core.Interfaces.IApplication.IUseCases;
using Ksignals.Apps.Auth.Core.Interfaces.IRepository;
using KSignals.Domain.Shared.UseCases;
using KSignals.Domain.Shared.ValueObjects.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSignals.Domain.Shared.ValueObjects.Person;
namespace Ksignals.Apps.Auth.Application.UseCases.LoginUserPass
{
    public class Handler(IAuthRepository repository) : ILoginUserPassHandler
    {
        private readonly IAuthRepository repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<Response> HandleAsync(Command command)
        {
            try
            {
                var password = new Password(command.Password);
                var user = command.Username ?? throw new ArgumentNullException(nameof(command.Username));
                var is_auth = await repository.AuthenticateUserAsync(command.Username, password);

                if (is_auth)
                {
                    return new Response(
                      Value: "token asdlfasçldfalsdj---alsdflçajl",
                      IsSuccess: true,
                      Error: null
                  );
                }
                return new Response(
                       Value: null,
                       IsSuccess: false,
                       Error: "Invalido!"
                   );
            }
            catch (Exception _ex)
            {
                return new Response(
                       Value: null,
                       IsSuccess: false,
                       Error: _ex.Message
                   );

            }

        }
    }
}
