using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpDemo.models
{
    public class Post
    {
        public string id { get; set; }
        public string author { get; set; }
        public string title { get; set; }

        public override string ToString()
        {
            return $"Post {id}:\nAuthor: {author}\nTitle: {title}";
        }
    }
}
