using Application.DTOs.Book;
using Application.DTOs.Result;

namespace Application.Interfaces;

public interface IBookService
{
    Task<OperationResult<string>> CreateBookAsync(BookRegisterDTO dto);
    Task<IEnumerable<BookResponseDTO>> GetAllBooksAsync(int skip = 0, int take = 10);
    Task<OperationResult<string>> UpdateBookAsync(string bookId, BookUpdateDTO dto);
    Task<OperationResult<string>> DeleteBookAsync(string bookId);
}