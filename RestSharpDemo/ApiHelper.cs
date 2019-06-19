using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpDemo
{
    public static class ApiHelper
    {
        public static async Task<IRestResponse<T>> RunRequestAsync<T>(this IRestClient client, IRestRequest request) where T : class, new()
        {
            TaskCompletionSource<IRestResponse<T>> task = new TaskCompletionSource<IRestResponse<T>>();
            client.ExecuteAsync<T>(request, response =>
            {
                if (response.ErrorException != null)
                {
                    throw new ApplicationException("Error occured on response", response.ErrorException);
                }
                task.SetResult(response);
            });
            return await task.Task;
        }
    }
}
