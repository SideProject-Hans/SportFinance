using FinanceCenter.Data.Entities;

namespace FinanceCenter.Repositories;

/// <summary>
/// 部門資料存取層介面
/// </summary>
public interface IDepartmentRepository
{
	Task<List<Department>> GetAllAsync();
	Task<List<Department>> GetActiveAsync();
	Task<Department?> GetByIdAsync(int id);
	Task<Department?> GetByCodeAsync(string code);
	void Add(Department department);
	void Update(Department department);
	void Delete(Department department);
}
