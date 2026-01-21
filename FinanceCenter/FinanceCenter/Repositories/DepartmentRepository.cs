using FinanceCenter.Data;
using FinanceCenter.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceCenter.Repositories;

/// <summary>
/// 部門資料存取層
/// </summary>
public class DepartmentRepository(FinanceCenterDbContext context) : IDepartmentRepository
{
	public async Task<List<Department>> GetAllAsync()
	{
		return await context.Departments
			.OrderBy(d => d.SortOrder)
			.ThenBy(d => d.Code)
			.ToListAsync();
	}

	public async Task<List<Department>> GetActiveAsync()
	{
		return await context.Departments
			.Where(d => d.IsActive)
			.OrderBy(d => d.SortOrder)
			.ThenBy(d => d.Code)
			.ToListAsync();
	}

	public async Task<Department?> GetByIdAsync(int id)
	{
		return await context.Departments.FindAsync(id);
	}

	public async Task<Department?> GetByCodeAsync(string code)
	{
		return await context.Departments
			.FirstOrDefaultAsync(d => d.Code == code);
	}

	public async Task<bool> ExistsByCodeAsync(string code, int? excludeId = null)
	{
		return await context.Departments
			.AsNoTracking()
			.AnyAsync(d => d.Code == code && (excludeId == null || d.Id != excludeId));
	}

	public void Add(Department department)
	{
		context.Departments.Add(department);
	}

	public void Update(Department department)
	{
		context.Departments.Update(department);
	}

	public void Delete(Department department)
	{
		context.Departments.Remove(department);
	}
}
