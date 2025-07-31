using KSignals.Domain.Shared.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSignals.Domain.Shared.ValueObjects.Results
{
    public record ResponseBase(bool IsSuccess, string? Error) : IResponse
    {
        public static ResponseBase Success() => new(true, null);
        public static ResponseBase Failure(string error) => new(false, error);
    }
    public record ResponseBase<T>(T? Value, bool IsSuccess, string? Error) : IResponse
    where T : notnull
    {
        public static ResponseBase<T> Success(T value) => new(value, true, null);
        public static ResponseBase<T> Failure(string error) => new(default, false, error);
    }
}
