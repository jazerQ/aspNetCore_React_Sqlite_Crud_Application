using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models; // подключаю модели которые я создал в проекте Core (модели это классы, созданные для бизнес логики)
using DataAccess.Entities; // также подключаю класс который является сущностью для базы данных это та же самая модель для бизнес логики только с учетом того, что там только поля
using Microsoft.EntityFrameworkCore; // подключаю собственно сам EntityFrameworkCore

namespace DataAccess.Repositories
{
    public class BookRepository : IBookRepository // класс репозитория будет реализовывать интерфейс IBookRepository
    {
        private readonly MVDbContext _dbContext; // добавляю приватное, readonly поле типа контекста для базы данных 
        public BookRepository(MVDbContext context) // в конструкторе я определяю этот контекст
        {
            _dbContext = context;
        }

        public async Task<List<Book>> GetBooks() // начинаю прописывать crud операции
        {
            var bookEntities = await _dbContext.Books //я достаю книжки из базы данных и привожу их к типу List
                .AsNoTracking() // важно! я не храню их в кэше, они не отслеживаются. тем самым я оптимизирую запрос.
                .ToListAsync();
            var books = bookEntities.Select(b => Book.Create(b.Id, b.Title, b.Description, b.Price).Book).ToList(); // создаю List типа Book
            return books; // возвращаю значение
        }
        public async Task<Book> GetBook(Guid id) 
        {
            var bookEntity = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (bookEntity != null)
            {
                var book = Book.Create(bookEntity.Id, bookEntity.Title, bookEntity.Description, bookEntity.Price).Book;
                return book;
            }
            throw new BookNotFoundException("Book not Found!");
        }
        public async Task<Guid> Create(Book book) // создаю новый метод для создания записи в базу данных
        {
            var bookEntity = new BookEntity { Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price 
            }; // создаю объект класса сущности для базы данных из тех данных, что передаются в конструкторе метода
            await _dbContext.AddAsync(bookEntity);// добавляю эту запись в базу данных
            await _dbContext.SaveChangesAsync(); // сохраняю изменения

            return bookEntity.Id; // возвращаю айдишник
        }
        public async Task<Guid> Update(Guid id, string title, string description, int price) // новый метод для обновления данных
        {
            await _dbContext.Books // обновляю
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.Title, b => title)
                    .SetProperty(b => b.Description, b => description)
                    .SetProperty(b => b.Price, b => price)
                    );
            return id;
        }
        public async Task<Guid> Delete(Guid id)// новый метод для удаления
        {
            await _dbContext.Books.Where(x => x.Id == id).ExecuteDeleteAsync(); // удаляю
            return id;
        }
    }
}
