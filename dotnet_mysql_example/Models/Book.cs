using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_mysql_example.Models
{
    public class Book
    {
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]

        public virtual Author? Author { get; set; }

        public override string ToString()
        {
            var txt = new StringBuilder();
            txt.AppendLine($"ISBN: {ISBN}");
            txt.AppendLine($"Title: {Title}");
            txt.AppendLine($"Author: {Author.Name}");

            return txt.ToString();
        }
    }
}
