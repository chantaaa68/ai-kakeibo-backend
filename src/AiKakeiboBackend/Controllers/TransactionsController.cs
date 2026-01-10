using AiKakeiboBackend.Data;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly KakeiboDbContext _context;

    public TransactionsController(KakeiboDbContext context)
    {
        _context = context;
    }

    // GET: api/Transactions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions(
        [FromQuery] int? userId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? categoryId)
    {
        var query = _context.Transactions
            .Include(t => t.Category)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(t => t.UserId == userId.Value);

        if (startDate.HasValue)
            query = query.Where(t => t.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(t => t.Date <= endDate.Value);

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        var transactions = await query
            .OrderByDescending(t => t.Date)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                Note = t.Note,
                Date = t.Date,
                Type = t.Type,
                UserId = t.UserId,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return Ok(transactions);
    }

    // GET: api/Transactions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
    {
        var transaction = await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.Id == id)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                Note = t.Note,
                Date = t.Date,
                Type = t.Type,
                UserId = t.UserId,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (transaction == null)
        {
            return NotFound();
        }

        return Ok(transaction);
    }

    // POST: api/Transactions
    [HttpPost]
    public async Task<ActionResult<TransactionDto>> CreateTransaction(CreateTransactionDto dto, [FromQuery] int userId)
    {
        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Description = dto.Description,
            Note = dto.Note,
            Date = dto.Date,
            Type = dto.Type,
            UserId = userId,
            CategoryId = dto.CategoryId
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        var createdTransaction = await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.Id == transaction.Id)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                Note = t.Note,
                Date = t.Date,
                Type = t.Type,
                UserId = t.UserId,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .FirstOrDefaultAsync();

        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, createdTransaction);
    }

    // PUT: api/Transactions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(int id, UpdateTransactionDto dto)
    {
        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction == null)
        {
            return NotFound();
        }

        if (dto.Amount.HasValue)
            transaction.Amount = dto.Amount.Value;

        if (dto.Description != null)
            transaction.Description = dto.Description;

        if (dto.Note != null)
            transaction.Note = dto.Note;

        if (dto.Date.HasValue)
            transaction.Date = dto.Date.Value;

        if (dto.Type.HasValue)
            transaction.Type = dto.Type.Value;

        if (dto.CategoryId.HasValue)
            transaction.CategoryId = dto.CategoryId.Value;

        transaction.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Transactions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction == null)
        {
            return NotFound();
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
