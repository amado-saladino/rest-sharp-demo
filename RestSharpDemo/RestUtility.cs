using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpDemo
{
    public class RestUtility
    {
        public static async Task<T> CallEndPointRequest<T>(string url, string operation, T requestBodyObject, string method, string username, string password) where T:class
        {
            // Initialize an HttpWebRequest for the current URL.
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method;
            request.ContentType = "application/json";

            //Add basic authentication header if username is supplied
            if (!string.IsNullOrEmpty(username))            
                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password));            

            //Add key to header if operation is supplied
            if (string.IsNullOrEmpty(operation))
                request.Headers["Operation"] = operation;
           
            //Serialize request object as JSON and write to request body
            if (requestBodyObject!=null)
            {
                string requestBodyString = JsonConvert.SerializeObject(requestBodyObject);
                request.ContentLength = requestBodyString.Length;
                StreamWriter stream = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
                stream.Write(requestBodyString);
                stream.Close();
            }
            WebResponse response = request.GetResponseAsync().GetAwaiter().GetResult();

            if (response == null)
                return default;

            string responseContent = new StreamReader( response.GetResponseStream() ).ReadToEndAsync().GetAwaiter().GetResult().Trim();
            return await Task<T>.Run(()=> JsonConvert.DeserializeObject<T>(responseContent));
        }
    }
}
