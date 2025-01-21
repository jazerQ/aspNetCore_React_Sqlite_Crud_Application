using Core.Exceptions;
using Core.Models;
using DataAccess.Repositories;

namespace Application
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository booksRepository)
        {
            _bookRepository = booksRepository;
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _bookRepository.GetBooks();
        }
        public async Task<Book> GetBook(Guid id)
        {
            try
            {
                return await _bookRepository.GetBook(id);
            }
            catch (BookNotFoundException ex)
            {
                Console.WriteLine($"Book has problem: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while fetching book: {ex.Message}");
                throw;
            }
            finally
            {
                Console.WriteLine("operation GetBook() is gone");
            }
        }
        public async Task<Guid> CreateBook(Book book)
        {
            return await _bookRepository.Create(book);
        }
        public async Task<Guid> UpdateBook(Guid id, string title, string description, int price)
        {
            return await _bookRepository.Update(id, title, description, price);
        }
        public async Task<Guid> DeleteBook(Guid id)
        {
            return await _bookRepository.Delete(id);
        }
    }
}
