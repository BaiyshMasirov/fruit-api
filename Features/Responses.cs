using Microsoft.AspNetCore.Mvc;

namespace fruit_api.Features;

public static class Responses
{
    public static JsonResult Ok(object data) =>
        new JsonResult(new { success = true, statusCode = 200, data, error = (object?)null });

    public static JsonResult Error(int statusCode, string errorCode, string message) =>
        new JsonResult(new
        {
            success = false, statusCode, data = (object?)null,
            error = new
            {
                errorCode, message,
                fieldsValidationErrors = Array.Empty<object>(),
                paramsErrors = Array.Empty<object>()
            }
        }) { StatusCode = statusCode };
}

