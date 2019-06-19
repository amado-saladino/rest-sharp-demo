using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharpDemo.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpDemo.RestUtilityTests
{
    class RestUtilityTest
    {
        private const string firstTodoURL = "https://jsonplaceholder.typicode.com/todos/1";
        private const string todosURL = "https://jsonplaceholder.typicode.com/todos";

        [Test]
        public async Task GetFirstTodoTest()
        {
            Todo firstTodo = await RestUtility.CallEndPointRequest<Todo>(firstTodoURL, string.Empty, null, "GET", string.Empty, string.Empty);
            TestContext.WriteLine(firstTodo);
            Assert.That(firstTodo.Id, Is.EqualTo(1));
            Assert.That(firstTodo.Title, Is.EqualTo("delectus aut autem"));
            Assert.IsFalse(firstTodo.Completed);
        }

        [Test]
        public async Task GetFirstTodoASJsonTest()
        {
            JObject firstTodo = await RestUtility.CallEndPointRequest<JObject>(firstTodoURL, string.Empty, null, "GET", string.Empty, string.Empty);
            TestContext.WriteLine(firstTodo);
            Assert.That((int)firstTodo["id"], Is.EqualTo(1));
            Assert.That(firstTodo["title"].ToString(), Is.EqualTo("delectus aut autem"));
            Assert.That((int)firstTodo["userId"], Is.EqualTo(1));
            Assert.IsFalse((bool)firstTodo["completed"]);
        }

        [Test]
        public async Task GetAllTodosTest()
        {
            IList<Todo> todos = await RestUtility.CallEndPointRequest<List<Todo>>(todosURL, string.Empty, null, "GET", string.Empty, string.Empty);
            Assert.That(todos.Count, Is.EqualTo(200));
            Assert.That(todos.First().Id, Is.EqualTo(1));
            Assert.That(todos.Last().Id, Is.EqualTo(200));
        }

        [Test]
        public async Task CreateTodoTest()
        {
            Todo todayTodo = new Todo() { Title = "Deutsch lernen", UserId = 1, Completed = false };
            Todo createdTodo = await RestUtility.CallEndPointRequest<Todo>(todosURL, string.Empty, todayTodo, "POST", string.Empty, string.Empty);

            TestContext.WriteLine($"Today's Todo:\n{todayTodo}");
            TestContext.WriteLine(new string('-', 10));
            TestContext.WriteLine($"Created Todo:\n{createdTodo}");
            Assert.That(createdTodo.Id, Is.EqualTo(201));
            Assert.That(createdTodo.Title, Is.EqualTo("Deutsch lernen"));
            Assert.IsFalse(createdTodo.Completed);
            Assert.AreEqual(todayTodo, createdTodo);
        }


    }
}
