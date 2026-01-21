using FinanceCenter.Data.Entities;
using FinanceCenter.Repositories;
using FinanceCenter.Services;
using Moq;

namespace FinanceCenter.Tests.Services;

/// <summary>
/// FinanceService 單元測試
/// </summary>
public class FinanceServiceTests
{
	private readonly Mock<IUnitOfWork> _mockUnitOfWork;
	private readonly Mock<IFinanceRepository> _mockRepository;
	private readonly FinanceService _service;

	public FinanceServiceTests()
	{
		_mockUnitOfWork = new Mock<IUnitOfWork>();
		_mockRepository = new Mock<IFinanceRepository>();
		_mockUnitOfWork.Setup(u => u.Finance).Returns(_mockRepository.Object);
		_service = new FinanceService(_mockUnitOfWork.Object);
	}

	[Fact]
	public async Task Should_ReturnAllCashFlows_When_GetAllCashFlowsAsync()
	{
		// Arrange
		var expected = new List<CashFlow>
		{
			new() { Id = 1, Department = "IT", Applicant = "John" },
			new() { Id = 2, Department = "HR", Applicant = "Jane" }
		};
		_mockRepository.Setup(r => r.GetAllCashFlowsAsync()).ReturnsAsync(expected);

		// Act
		var result = await _service.GetAllCashFlowsAsync();

		// Assert
		Assert.Equal(2, result.Count);
		Assert.Equal(expected, result);
		_mockRepository.Verify(r => r.GetAllCashFlowsAsync(), Times.Once);
	}

	[Fact]
	public async Task Should_AddCashFlowAndSave_When_AddCashFlowAsync()
	{
		// Arrange
		var cashFlow = new CashFlow
		{
			Department = "IT",
			Applicant = "John",
			Reason = "Test",
			Income = 1000
		};
		_mockUnitOfWork.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

		// Act
		var result = await _service.AddCashFlowAsync(cashFlow);

		// Assert
		Assert.Equal(cashFlow, result);
		_mockRepository.Verify(r => r.Add(cashFlow), Times.Once);
		_mockUnitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task Should_ReturnFalse_When_DeleteNonExistentCashFlow()
	{
		// Arrange
		_mockRepository.Setup(r => r.GetCashFlowByIdAsync(999)).ReturnsAsync((CashFlow?)null);

		// Act
		var result = await _service.DeleteCashFlowAsync(999);

		// Assert
		Assert.False(result);
		_mockRepository.Verify(r => r.Delete(It.IsAny<CashFlow>()), Times.Never);
		_mockUnitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Never);
	}

	[Fact]
	public async Task Should_DeleteAndReturnTrue_When_DeleteExistingCashFlow()
	{
		// Arrange
		var cashFlow = new CashFlow { Id = 1, Department = "IT", Applicant = "John", Reason = "Test" };
		_mockRepository.Setup(r => r.GetCashFlowByIdAsync(1)).ReturnsAsync(cashFlow);
		_mockUnitOfWork.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

		// Act
		var result = await _service.DeleteCashFlowAsync(1);

		// Assert
		Assert.True(result);
		_mockRepository.Verify(r => r.Delete(cashFlow), Times.Once);
		_mockUnitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task Should_UpdateCashFlowAndSave_When_UpdateCashFlowAsync()
	{
		// Arrange
		var existingCashFlow = new CashFlow
		{
			Id = 1,
			Department = "HR",
			Applicant = "Jane",
			Reason = "Original",
			Income = 1000
		};
		var updatedCashFlow = new CashFlow
		{
			Id = 1,
			Department = "IT",
			Applicant = "John",
			Reason = "Updated",
			Income = 2000
		};
		_mockRepository.Setup(r => r.GetCashFlowByIdAsync(1)).ReturnsAsync(existingCashFlow);
		_mockUnitOfWork.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

		// Act
		var result = await _service.UpdateCashFlowAsync(updatedCashFlow);

		// Assert
		Assert.Equal(existingCashFlow, result);
		Assert.Equal("IT", existingCashFlow.Department);
		Assert.Equal("John", existingCashFlow.Applicant);
		Assert.Equal("Updated", existingCashFlow.Reason);
		Assert.Equal(2000, existingCashFlow.Income);
		_mockUnitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once);
	}

	[Fact]
	public async Task Should_ReturnCashFlow_When_GetCashFlowByIdAsync()
	{
		// Arrange
		var expected = new CashFlow { Id = 1, Department = "IT", Applicant = "John", Reason = "Test" };
		_mockRepository.Setup(r => r.GetCashFlowByIdAsync(1)).ReturnsAsync(expected);

		// Act
		var result = await _service.GetCashFlowByIdAsync(1);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(expected, result);
	}

	[Fact]
	public async Task Should_ReturnNull_When_GetCashFlowByIdAsyncNotFound()
	{
		// Arrange
		_mockRepository.Setup(r => r.GetCashFlowByIdAsync(999)).ReturnsAsync((CashFlow?)null);

		// Act
		var result = await _service.GetCashFlowByIdAsync(999);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task Should_ReturnFilteredCashFlows_When_GetCashFlowsByDepartmentAsync()
	{
		// Arrange
		var expected = new List<CashFlow>
		{
			new() { Id = 1, Department = "IT", Applicant = "John", Reason = "Test" }
		};
		_mockRepository.Setup(r => r.GetCashFlowsByDepartmentAsync("IT")).ReturnsAsync(expected);

		// Act
		var result = await _service.GetCashFlowsByDepartmentAsync("IT");

		// Assert
		Assert.Single(result);
		Assert.All(result, cf => Assert.Equal("IT", cf.Department));
	}

	[Fact]
	public async Task Should_ReturnFilteredCashFlows_When_GetCashFlowsByDateRangeAsync()
	{
		// Arrange
		var startDate = new DateTime(2024, 1, 1);
		var endDate = new DateTime(2024, 12, 31);
		var expected = new List<CashFlow>
		{
			new() { Id = 1, Department = "IT", Applicant = "John", Reason = "Test", CreateDay = new DateTime(2024, 6, 15) }
		};
		_mockRepository.Setup(r => r.GetCashFlowsByDateRangeAsync(startDate, endDate)).ReturnsAsync(expected);

		// Act
		var result = await _service.GetCashFlowsByDateRangeAsync(startDate, endDate);

		// Assert
		Assert.Single(result);
		_mockRepository.Verify(r => r.GetCashFlowsByDateRangeAsync(startDate, endDate), Times.Once);
	}
}
