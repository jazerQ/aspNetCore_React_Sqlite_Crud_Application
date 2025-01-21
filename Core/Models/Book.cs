using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Book
    {
        public const int MAX_TITLE_LENGTH = 250;
        public Book(Guid id, string title, string description, decimal price)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
        }
        public Guid Id { get; }
        public string Title { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public decimal Price { get; }

        public static (Book Book, string Error) Create(Guid id, string title, string description, decimal price)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(title) || MAX_TITLE_LENGTH < title.Length)
            {
                error = "title can not be empty or title length can not be more then 250 letters";
            }
            var book = new Book(id, title, description, price);
            return (book, error);
        }
    }
}
