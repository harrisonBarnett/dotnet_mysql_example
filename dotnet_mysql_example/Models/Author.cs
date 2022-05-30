using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_mysql_example.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Book>? Books { get; set; }

        public override string ToString()
        {
            var txt = new StringBuilder();
            txt.AppendLine($"ID: {AuthorId}");
            txt.AppendLine($"Name: {Name}");

            return txt.ToString();
        }
    }
}
