namespace CoolBytes.Core.Utils
{
    public static class ResultExtensions
    {
        public static Result<T> ToSuccessResult<T>(this T payload)
            => Result<T>.Success(payload);
    }
}