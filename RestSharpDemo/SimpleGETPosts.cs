using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using RestSharpDemo.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestSharpDemo
{
    class SimpleGETPosts
    {
        IRestClient client;
        IRestResponse response;
        JsonDeserializer deserialzer;

        [OneTimeSetUp]
        public void setup()
        {
            client = new RestClient("http://localhost:3000");
            deserialzer = new JsonDeserializer();
        }
        [Test]
        public void getFirstPost()
        {
            IRestRequest request = new RestRequest("posts/{postid}", Method.GET);
            request.AddUrlSegment("postid", "1");
            response = client.Execute(request);
            string content = response.Content;
            TestContext.WriteLine(content);

            IDictionary<string, string> jsonObject = deserialzer.Deserialize<Dictionary<string, string>>(response);
            string author = jsonObject["author"];
            TestContext.WriteLine($"Author coming from the server: {author}");

            Post post = JsonConvert.DeserializeObject<Post>(content);
            TestContext.WriteLine(post);

            JObject jObject = JObject.Parse(response.Content);
            string title = jObject["title"].ToString();
            TestContext.WriteLine($"Title coming from the response: {title}");
        }
    }
}
