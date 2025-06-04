using Application.DTOs.Book;
using Application.DTOs.Result;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAWSS3Service _awsS3Service;

    public BookService(IBookRepository bookRepository, IAWSS3Service awsS3Service)
    {
        _bookRepository = bookRepository;
        _awsS3Service = awsS3Service;
    }

    public async Task<OperationResult<string>> CreateBookAsync(Guid userId, BookRegisterDTO dto)
    {
        try
        {
            if (dto.Status == StatusLeitura.Lido && (dto.Nota is < 1 or > 5))
                return OperationResult<string>.Failure("Nota deve ser entre 1 e 5 para livros lidos.");

            var book = new Book
            {
                UsuarioId = userId,
                Nome = dto.Nome,
                Autor = dto.Autor,
                Sinopse = dto.Sinopse,
                Status = dto.Status,
                Nota = dto.Nota
            };

            if (dto.Capa != null)
            {
                var key = "medias/" + Guid.NewGuid();
                await _awsS3Service.UploadImage("clube-livro", key, dto.Capa);
                book.AddImageKey(key);
            }

            await _bookRepository.AddAsync(book);

            return OperationResult<string>.Success("Livro cadastrado com sucesso.");
        }
        catch
        {
            return OperationResult<string>.Failure("Ocorreu um erro.");
        }
    }

    public async Task<IEnumerable<BookResponseDTO>> GetAllBooksAsync(Guid userId)
    {
        var books = await _bookRepository.GetAllBooksFromUser(userId);

        return books.Select(l => new BookResponseDTO
        {
            Id = l.Id,
            Nome = l.Nome,
            Autor = l.Autor,
            Sinopse = l.Sinopse,
            Status = l.Status,
            CapaUrl = l.CapaUrl,
            Nota = l.Nota
        }).ToList();
    }

    public async Task<OperationResult<string>> UpdateBookAsync(Guid userId, string bookId, BookUpdateDTO dto)
    {
        var book = await _bookRepository.ConsultBook(bookId, userId);

        if (book == null)
            return OperationResult<string>.Failure("Livro não encontrado.");

        if (!string.IsNullOrEmpty(dto.Nome))
            book.Nome = dto.Nome;

        if (!string.IsNullOrEmpty(dto.Autor))
            book.Autor = dto.Autor;

        if (!string.IsNullOrEmpty(dto.Sinopse))
            book.Sinopse = dto.Sinopse;

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

        if (dto.CapaUrl != null)
        {
            var key = "medias/" + Guid.NewGuid();
            await _awsS3Service.UploadImage("clube-livro", key, dto.CapaUrl);
            book.AddImageKey(key);
        }

        await _bookRepository.UpdateAsync(book);

        return OperationResult<string>.Success("Livro atualizado com sucesso.");
    }

    public async Task<OperationResult<string>> DeleteBookAsync(Guid userId, string bookId)
    {
        var book = await _bookRepository.ConsultBook(bookId, userId);

        if (book == null)
            return OperationResult<string>.Failure("Livro não encontrado.");

        await _bookRepository.DeleteAsync(book);
        return OperationResult<string>.Success("Livro excluído com sucesso.");
    }
}