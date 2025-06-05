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

    public async Task<OperationResult<string>> CreateBookAsync(BookRegisterDTO dto)
    {
        try
        {
            var book = new Book
            {
                Nome = dto.Nome,
                Autor = dto.Autor,
                Sinopse = dto.Sinopse
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

    public async Task<IEnumerable<BookResponseDTO>> GetAllBooksAsync(int skip = 0, int take = 10)
    {
        var books = await _bookRepository.GetAllBooks(skip, take);

        return books.Select(l => new BookResponseDTO
        {
            Id = l.Id,
            Nome = l.Nome,
            Autor = l.Autor,
            Sinopse = l.Sinopse,
            CapaUrl = l.CapaUrl
        }).ToList();
    }

    public async Task<OperationResult<string>> UpdateBookAsync(string bookId, BookUpdateDTO dto)
    {
        var book = await _bookRepository.ConsultBookById(bookId);

        if (book == null)
            return OperationResult<string>.Failure("Livro não encontrado.");

        if (!string.IsNullOrEmpty(dto.Nome))
            book.Nome = dto.Nome;

        if (!string.IsNullOrEmpty(dto.Autor))
            book.Autor = dto.Autor;

        if (!string.IsNullOrEmpty(dto.Sinopse))
            book.Sinopse = dto.Sinopse;

        if (dto.CapaUrl != null)
        {
            var key = "medias/" + Guid.NewGuid();
            await _awsS3Service.UploadImage("clube-livro", key, dto.CapaUrl);
            book.AddImageKey(key);
        }

        await _bookRepository.UpdateAsync(book);

        return OperationResult<string>.Success("Livro atualizado com sucesso.");
    }

    public async Task<OperationResult<string>> DeleteBookAsync(string bookId)
    {
        var book = await _bookRepository.ConsultBookById(bookId);

        if (book == null)
            return OperationResult<string>.Failure("Livro não encontrado.");

        await _bookRepository.DeleteAsync(book);
        return OperationResult<string>.Success("Livro excluído com sucesso.");
    }
}