using System;

namespace CoolBytes.Core.Utils
{
    public abstract class Result<T> : Result
    {
        public T Payload { get; protected set; }

        public new bool Is<T2>() where T2 : Result<T> => this is T2;
        
        public new T2 As<T2>() where T2 : Result<T> => this as T2;
        
        public new static Result<T> NotFoundResult() => new NotFoundResult<T>();

        public new static Result<T> ExceptionResult(Exception exception) => new ExceptionResult<T>(exception);

        public new static Result<T> ErrorResult(string error) => new ErrorResult<T>(error);
        
        public static Result<T> Success(T payload) => new SuccessResult<T>(payload);
    }

    public abstract class Result
    {
        public bool IsSuccess { get; protected set; }
        
        public bool Is<T>() where T : Result => this is T;
        
        public T As<T>() where T : Result => this as T;

        public static Result SuccessResult() => new SuccessResult();

        public static Result NotFoundResult() => new NotFoundResult();
        
        public static Result ExceptionResult(Exception exception) => new ExceptionResult(exception);

        public static Result ErrorResult(string error) => new ErrorResult(error);

        public static implicit operator bool(Result result)
        {
            return result != null && result.IsSuccess;
        }
    }
}