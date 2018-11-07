using Microsoft.AspNetCore.Mvc;

namespace CoolBytes.WebAPI.Extensions
{
    public static class ControllerExtensions
    {
        public static ActionResult<T> OkOrNotFound<T>(this ControllerBase controller, T model)
        {
            if (model != null)
                return model;

            return controller.NotFound();
        }
    }
}