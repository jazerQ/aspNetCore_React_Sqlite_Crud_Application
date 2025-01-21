using Core.Models;

namespace Application
{
    public interface IBookService
    {
        Task<Guid> CreateBook(Book book);
        Task<Guid> DeleteBook(Guid id);
        Task<List<Book>> GetAllBooks();
        Task<Book> GetBook(Guid id);
        Task<Guid> UpdateBook(Guid id, string title, string description, decimal price);
    }
}