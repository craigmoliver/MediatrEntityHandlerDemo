using System;
using System.Collections.Generic;
using MediatrEntityHandlerDemo.Domain.Enums;
using Microsoft.ApplicationInsights;

namespace MediatrEntityHandlerDemo.Handlers
{
    public interface IResult
    {
        bool Success { get; }
        List<string> Errors { get; }
        bool HasNoErrors { get; }
        ErrorTypeEnum ErrorType { get; set; }

        void SetValid();
        IResult AddAndTrackError(TelemetryClient client, string correlationId, Exception ex, params KeyValuePair<string, string>[] errors);
        IResult AddAndTrackError(TelemetryClient client, string correlationId, string exceptionMessage, params KeyValuePair<string, string>[] errors);
        IResult AddError(string error);
        IResult AddErrors(List<string> errors);
    }

    public interface IResult<T> : IResult
    {
        void SetValue(T value);

        T Value { get; }
    }
}
