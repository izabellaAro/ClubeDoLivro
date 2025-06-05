using Application.DTOs.BookShelf;
using Application.DTOs.Result;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class BookShelfService : IBookShelfService
{
    private readonly IBookShelfRepository _shelfRepository;
    private readonly IBookRepository _bookRepository;

    public BookShelfService(IBookShelfRepository shelfRepository, IBookRepository bookRepository)
    {
        _shelfRepository = shelfRepository;
        _bookRepository = bookRepository;
    }

    public async Task<OperationResult<string>> AddBookToShelfAsync(Guid userId, string bookId, BookShelfRegisterDTO dto)
    {
        var book = await _bookRepository.ConsultBookById(bookId);
        if (book == null)
            return OperationResult<string>.Failure("Livro não encontrado.");

        var existingBook = await _shelfRepository.ConsultBookShelfById(userId, bookId);

        if (existingBook != null)
            return OperationResult<string>.Failure("Livro já adicionado na sua estante.");

        if (dto.Status == StatusLeitura.Lido && dto.Nota is < 1 or > 5)
            return OperationResult<string>.Failure("Nota deve ser entre 1 e 5 para livros lidos.");

        var bookShelf = new BookShelf
        {
            UsuarioId = userId,
            LivroId = bookId,
            Status = dto.Status,
            Nota = dto.Status == StatusLeitura.Lido ? dto.Nota : null
        };

        await _shelfRepository.AddAsync(bookShelf);
        return OperationResult<string>.Success("Livro adicionado na sua estante.");
    }

    public async Task<IEnumerable<BookShelfResponseDTO>> GetAllBooksShelfAsync(Guid userId)
    {
        var books = await _shelfRepository.GetAllBooksShelf(userId);

        return books.Select(x => new BookShelfResponseDTO
        {
            LivroId = x.LivroId,
            Nome = x.Livro.Nome,
            Autor = x.Livro.Autor,
            Sinopse = x.Livro.Sinopse,
            CapaUrl = x.Livro.CapaUrl,
            Status = x.Status,
            Nota = x.Nota
        }).ToList();
    }

    public async Task<OperationResult<string>> UpdateBookShelfAsync(Guid userId, string bookId, BookShelfUpdateDTO dto)
    {
        var book = await _shelfRepository.ConsultBookShelfById(userId, bookId);

        if (book == null)
            return OperationResult<string>.Failure("Livro não encontrado na sua estante.");

        if (dto.Status.HasValue)
        {
            book.Status = dto.Status.Value;
            book.Nota = book.Status == StatusLeitura.Lido ? book.Nota : null;
        }

        if (dto.Nota.HasValue)
        {
            if (dto.Nota is < 1 or > 5)
                return OperationResult<string>.Failure("Nota deve ser entre 1 e 5.");

            if (book.Status == StatusLeitura.Lido)
                book.Nota = dto.Nota;
            else
                return OperationResult<string>.Failure("Só é possível atribuir nota se o livro estiver como 'Lido'.");
        }

        await _shelfRepository.UpdateAsync(book);

        return OperationResult<string>.Success("Estante atualizada com sucesso.");
    }

    public async Task<OperationResult<string>> DeleteBookToShelfAsync(Guid userId, string bookId)
    {
        var item = await _shelfRepository.ConsultBookShelfById(userId, bookId);

        if (item == null)
            return OperationResult<string>.Failure("Livro não encontrado na sua estante.");

        await _shelfRepository.DeleteAsync(item);

        return OperationResult<string>.Success("Livro removido da estante.");
    }
}
