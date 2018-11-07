using System.Collections.Generic;

namespace CoolBytes.WebAPI.Utils
{
    public class ErrorResult<T> : Result<T>
    {
        public string ErrorMessage { get; set; }

        public ErrorResult(string error)
        {
            ErrorMessage = error;
        }
    }

    public class ErrorResult : Result
    {
        public string ErrorMessage { get; }
        
        public ErrorResult(string error)
        {
            ErrorMessage = error;
        }
        
        public ErrorResult(IEnumerable<string> errors, char separator = ' ')
        {
            ErrorMessage = string.Join(separator, errors);
        }
    }
}