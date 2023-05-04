using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatrEntityHandlerDemo.Handlers
{
    public class BaseMessage : IBaseMessage
    {
        public string CorrelationId { get; }

        public IResult Result { get; }

        public BaseMessage()
        {
            CorrelationId = Guid.NewGuid().ToString();
            Result = new Result();
        }

        public BaseMessage(string correlationId)
        {
            CorrelationId = correlationId;
            Result = new Result();
        }

        public BaseMessage(string correlationId, IResult result)
        {
            CorrelationId = correlationId;
            Result = result;
        }
    }

    public class BaseMessage<T> : BaseMessage
    {
        public new IResult<T> Result { get; }

        public BaseMessage(string correlationId) : base(correlationId)
        {
            Result = new Result<T>();
        }

        public BaseMessage(string correlationId, IResult<T> result) : base(correlationId)
        {
            Result = result;
        }
    }
}
