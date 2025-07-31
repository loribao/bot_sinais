using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSignals.Domain.Shared.UseCases
{
    public interface IHandler<in TCommand, TResponse>
            where TCommand : ICommand
            where TResponse : IResponse
    {
        /// <summary>
        /// Handles the command and returns a response.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <returns>A response containing the result of the command execution.</returns>
        Task<TResponse> HandleAsync(TCommand command);
    }
}
