namespace AiKakeiboBackend.DTOs;

public class BudgetDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateBudgetDto
{
    public decimal Amount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int CategoryId { get; set; }
}

public class UpdateBudgetDto
{
    public decimal? Amount { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public int? CategoryId { get; set; }
}
