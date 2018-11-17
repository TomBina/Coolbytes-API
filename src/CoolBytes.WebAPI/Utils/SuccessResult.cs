namespace CoolBytes.WebAPI.Utils
{
    public class SuccessResult<T> : Result<T>
    {
        public SuccessResult(T payload)
        {
            IsSuccess = true;
            Payload = payload;
        }
    }

    public class SuccessResult : Result
    {
        public SuccessResult()
        {
            IsSuccess = true;
        }
    }
}