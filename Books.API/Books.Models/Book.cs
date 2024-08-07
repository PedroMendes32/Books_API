using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public virtual ICollection<Author>? Authors { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public virtual Publisher? Publisher { get; set; }

        public Book()
        {
              
        }

    }
}
