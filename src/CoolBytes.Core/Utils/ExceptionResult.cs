using System;

namespace CoolBytes.Core.Utils
{
    public class ExceptionResult<T> : Result<T>
    {
        public Exception Exception { get; }

        public ExceptionResult(Exception exception)
        {
            Exception = exception;
        }
    }

    public class ExceptionResult : Result
    {
        public Exception Error { get; }

        public ExceptionResult(Exception error)
        {
            Error = error;
        }
    }
}