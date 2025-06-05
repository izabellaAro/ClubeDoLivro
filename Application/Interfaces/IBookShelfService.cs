using Application.DTOs.BookShelf;
using Application.DTOs.Result;

namespace Application.Interfaces;

public interface IBookShelfService
{
    Task<OperationResult<string>> AddBookToShelfAsync(Guid userId, string bookId, BookShelfRegisterDTO dto);
    Task<IEnumerable<BookShelfResponseDTO>> GetAllBooksShelfAsync(Guid userId);
    Task<OperationResult<string>> UpdateBookShelfAsync(Guid userId, string bookId, BookShelfUpdateDTO dto);
    Task<OperationResult<string>> DeleteBookToShelfAsync(Guid userId, string bookId);
}