using Application.DTOs.Book;
using Application.DTOs.Result;

namespace Application.Interfaces;

public interface IBookService
{
    Task<OperationResult<string>> CreateBookAsync(Guid userId, BookRegisterDTO dto);
    Task<IEnumerable<BookResponseDTO>> GetAllBooksAsync(Guid userId);
    Task<OperationResult<string>> UpdateBookAsync(Guid userId, string bookId, BookUpdateDTO dto);
    Task<OperationResult<string>> DeleteBookAsync(Guid userId, string bookId);
}
