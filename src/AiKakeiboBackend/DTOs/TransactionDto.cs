using AiKakeiboBackend.Models;

namespace AiKakeiboBackend.DTOs;

public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Note { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateTransactionDto
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Note { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public int CategoryId { get; set; }
}

public class UpdateTransactionDto
{
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public DateTime? Date { get; set; }
    public TransactionType? Type { get; set; }
    public int? CategoryId { get; set; }
}
