using AiKakeiboBackend.Data;
using AiKakeiboBackend.DTOs;
using AiKakeiboBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiKakeiboBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetsController : ControllerBase
{
    private readonly KakeiboDbContext _context;

    public BudgetsController(KakeiboDbContext context)
    {
        _context = context;
    }

    // GET: api/Budgets
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetDto>>> GetBudgets(
        [FromQuery] int? userId,
        [FromQuery] int? year,
        [FromQuery] int? month)
    {
        var query = _context.Budgets
            .Include(b => b.Category)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(b => b.UserId == userId.Value);

        if (year.HasValue)
            query = query.Where(b => b.Year == year.Value);

        if (month.HasValue)
            query = query.Where(b => b.Month == month.Value);

        var budgets = await query
            .Select(b => new BudgetDto
            {
                Id = b.Id,
                Amount = b.Amount,
                Month = b.Month,
                Year = b.Year,
                UserId = b.UserId,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .ToListAsync();

        return Ok(budgets);
    }

    // GET: api/Budgets/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetDto>> GetBudget(int id)
    {
        var budget = await _context.Budgets
            .Include(b => b.Category)
            .Where(b => b.Id == id)
            .Select(b => new BudgetDto
            {
                Id = b.Id,
                Amount = b.Amount,
                Month = b.Month,
                Year = b.Year,
                UserId = b.UserId,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (budget == null)
        {
            return NotFound();
        }

        return Ok(budget);
    }

    // POST: api/Budgets
    [HttpPost]
    public async Task<ActionResult<BudgetDto>> CreateBudget(CreateBudgetDto dto, [FromQuery] int userId)
    {
        var budget = new Budget
        {
            Amount = dto.Amount,
            Month = dto.Month,
            Year = dto.Year,
            UserId = userId,
            CategoryId = dto.CategoryId
        };

        _context.Budgets.Add(budget);
        await _context.SaveChangesAsync();

        var createdBudget = await _context.Budgets
            .Include(b => b.Category)
            .Where(b => b.Id == budget.Id)
            .Select(b => new BudgetDto
            {
                Id = b.Id,
                Amount = b.Amount,
                Month = b.Month,
                Year = b.Year,
                UserId = b.UserId,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            })
            .FirstOrDefaultAsync();

        return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, createdBudget);
    }

    // PUT: api/Budgets/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudget(int id, UpdateBudgetDto dto)
    {
        var budget = await _context.Budgets.FindAsync(id);

        if (budget == null)
        {
            return NotFound();
        }

        if (dto.Amount.HasValue)
            budget.Amount = dto.Amount.Value;

        if (dto.Month.HasValue)
            budget.Month = dto.Month.Value;

        if (dto.Year.HasValue)
            budget.Year = dto.Year.Value;

        if (dto.CategoryId.HasValue)
            budget.CategoryId = dto.CategoryId.Value;

        budget.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Budgets/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(int id)
    {
        var budget = await _context.Budgets.FindAsync(id);

        if (budget == null)
        {
            return NotFound();
        }

        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
