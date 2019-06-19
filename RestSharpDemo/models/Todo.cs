using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpDemo.models
{
    public class Todo
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }

        public override string ToString()
        {
            return $"UserId ID: {UserId }\nID: {Id}\nTitle: {Title}\nCompleted: {Completed}";
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Todo);
        }

        public bool Equals(Todo otherTodo)
        {
            // If parameter is null, return false.
            if (Object.ReferenceEquals(otherTodo, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, otherTodo))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != otherTodo.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (UserId == otherTodo.UserId) 
                && (Title.Equals(otherTodo.Title)) && (Completed== otherTodo.Completed);
        }

        public override int GetHashCode()
        {
            return Id * 0x00010000 + UserId;
        }

        public static bool operator ==(Todo lhs, Todo rhs)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Todo lhs, Todo rhs)
        {
            return !(lhs == rhs);
        }
    }
}
