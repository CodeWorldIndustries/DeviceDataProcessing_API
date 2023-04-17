using Microsoft.AspNetCore.Mvc;

namespace CrossCutting.Common.Response
{
    public static class ObjectResultResponse
    {
        public static ObjectResult Error(int status, string errorMessage)
        {
            return new ObjectResult(new Response<string> { Data = null, Errored = true, ErrorMessage = errorMessage }) { StatusCode = status };
        }
    }

    public static class ObjectResultResponse<T>
    {
        public static ObjectResult Error(T result, int status, string errorMessage)
        {
            return new ObjectResult(new Response<T> { Data = result, Errored = true, ErrorMessage = errorMessage }) { StatusCode = status };
        }
    }

}
