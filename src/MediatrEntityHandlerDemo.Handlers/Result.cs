using System;
using System.Collections.Generic;
using System.Linq;
using MediatrEntityHandlerDemo.Domain.Enums;
using Microsoft.ApplicationInsights;

namespace MediatrEntityHandlerDemo.Handlers
{
    public class Result : IResult
    {
        public bool Success { get; private set; }
        public List<string> Errors { get; protected set; }

        public bool HasNoErrors => Errors.Count == 0;
        public ErrorTypeEnum ErrorType { get; set; }
        public Result()
        {
            Errors = new List<string>();
        }

        public virtual void SetValid()
        {
            Success = true;
        }

        public IResult AddAndTrackError(TelemetryClient client, string correlationId, Exception ex, params KeyValuePair<string, string>[] errors)
        {
            var dictionary = new Dictionary<string, string>
            {
                { "CorrelationId", correlationId },
                { "Exception", ex.Message }
            };

            if (errors != null && errors.Length > 0)
            {
                foreach (var keyValuePair in errors)
                {
                    dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            client.TrackException(ex, dictionary);

            // Prevent adding this same error if another error has already been added
            if (Errors.Contains($"CorrelationId: {correlationId}"))
            {
                dictionary.Remove("CorrelationId");
            }

            AddErrors(dictionary.Select(x => $"{x.Key}: {x.Value}").ToList());
            return this;
        }

        public IResult AddAndTrackError(TelemetryClient client, string correlationId, string exceptionMessage,
            params KeyValuePair<string, string>[] errors)
        {
            return AddAndTrackError(client, correlationId, new Exception(exceptionMessage), errors);
        }

        public virtual IResult AddError(string error)
        {
            Errors ??= new List<string>();
            Errors.Add(error);
            Success = false;
            return this;
        }

        public virtual IResult AddErrors(List<string> errors)
        {
            Errors ??= new List<string>();
            Errors.AddRange(errors);
            Success = false;
            return this;
        }
    }

    public class Result<T> : Result, IResult<T>
    {
        public T Value { get; private set; }

        public void SetValue(T value)
        {
            Value = value;
        }
    }
}
