using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using RestSharpDemo.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpDemo
{    
    class PostRequests
    {
        IRestClient client;
        JsonDeserializer deserializer;

        [OneTimeSetUp]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
            deserializer = new JsonDeserializer();
        }

        [Test]
        public void PostAnonymousBody()
        {
            string postId = "1"; string name = "Ahmed";
            IRestRequest request = new RestRequest("posts/{postid}/profile", DataFormat.Json);
            request.AddUrlSegment("postid", postId);
            request.AddBody(new { name = name });

            IRestResponse response = client.Execute(request, Method.POST);
            IDictionary<string,string> results = deserializer.Deserialize<Dictionary<string, string>>(response);

            TestContext.WriteLine($"Response:\n {response.Content}");
            Assert.AreEqual(name, results["name"]);
            Assert.AreEqual(postId, results["postId"]);
            Assert.That(response.StatusCode, Is.EqualTo(201), "Profile not added");
        }

        [Test]
        public void PostTypedBody()
        {
            string author = "Islam", title = "Dot Net Code";
            IRestRequest request = new RestRequest("posts", DataFormat.Json);
            request.AddBody(new Post() { author = author, title = title });
            IRestResponse response = client.Execute(request, Method.POST);
            TestContext.WriteLine($"Response:\n {response.Content}");

            IDictionary<string,string> results = deserializer.Deserialize<Dictionary<string, string>>(response);
            Assert.That(results["author"], Is.EqualTo(author));
            Assert.That(results["title"], Is.EqualTo(title));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "post not added");
        }

        [Test]
        public void PostGenericTypedBody()
        {
            string author = "Ahmed", title = "Automatiob testing";
            IRestRequest request = new RestRequest("posts", DataFormat.Json);
            request.AddBody(new Post() { author = author, title = title });
            Post responsePost = client.Execute<Post>(request, Method.POST).Data;
            Assert.That(responsePost.title, Is.EqualTo(title));
            Assert.That(responsePost.author, Is.EqualTo(author));
        }

        [Test]
        public void PostGenericTypedBodyAsync()
        {
            string author = "Ezzo", title = "Scrum process";
            IRestRequest request = new RestRequest("posts", DataFormat.Json);
            request.Method = Method.POST;
            request.AddBody(new Post() { author = author, title = title });
            IRestResponse<Post> response = RunRequestAsync<Post>(client, request).GetAwaiter().GetResult();
            Assert.That(response.Data.title, Is.EqualTo(title));
            Assert.That(response.Data.author, Is.EqualTo(author));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void PostGenericTypedBodyExtensionMethodAsync()
        {
            string author = "Samah", title = "TOSCA automation";
            IRestRequest request = new RestRequest("posts", DataFormat.Json);
            request.Method = Method.POST;
            request.AddBody(new Post() { author = author, title = title });
            IRestResponse<Post> response = client.RunRequestAsync<Post>(request).GetAwaiter().GetResult();
            Assert.That(response.Data.title, Is.EqualTo(title));
            Assert.That(response.Data.author, Is.EqualTo(author));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        private async Task<IRestResponse<T>> RunRequestAsync<T>(IRestClient client, IRestRequest request) where T:class,new()
        {
            TaskCompletionSource<IRestResponse<T>> task= new TaskCompletionSource<IRestResponse<T>>();
            client.ExecuteAsync<T>(request, response => 
            {
                if (response.ErrorException!=null)
                {
                    throw new ApplicationException("Error occured on response", response.ErrorException);
                }
                task.SetResult(response);
            });
            return await task.Task;
        }
    }
}
