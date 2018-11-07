namespace CoolBytes.WebAPI.Utils
{
    public class NotFoundResult<T> : Result<T>
    {
        public NotFoundResult()
        {
            IsSuccess = false;
        }
    }

    public class NotFoundResult : Result
    {
        public NotFoundResult()
        {
            IsSuccess = false;
        }
    }
}