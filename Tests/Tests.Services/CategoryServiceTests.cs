using System.Linq.Expressions;

namespace OpenMovies.Services.Tests;

public class CategoryServiceTests
{
    [Fact]
    public async Task CreateCategory_SuccessfulCreation_ShouldCreateCategory()
    {
        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object);

        var validCategory = new Category("Action");

        await categoryService.CreateCategory(validCategory);
        mockRepository.Verify(repo => repo.AddAsync(validCategory), Times.Once);
    }

    [Fact]
    public async Task CreateCategory_InvalidData_ShouldThrowValidationException()
    {
        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object);

        var invalidCategory = new Category("");

        await Assert.ThrowsAsync<ValidationException>(() => categoryService.CreateCategory(invalidCategory));
        mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task CreateCategory_DuplicateName_ShouldThrowInvalidOperationException()
    {
        var mockRepository = new Mock<ICategoryRepository>();
        mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(new Category("Action"));

        var categoryService = new CategoryService(mockRepository.Object);
        var existingCategory = new Category("Action");

        await Assert.ThrowsAsync<InvalidOperationException>(() => categoryService.CreateCategory(existingCategory));
        mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturnAllCategories()
    {
        var mockRepository = new Mock<ICategoryRepository>();
        var categories = new List<Category> { new Category("Action"), new Category("Drama") };

        mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);
        var categoryService = new CategoryService(mockRepository.Object);

        var result = await categoryService.GetAllCategories();

        Assert.NotNull(result);
        Assert.Equal(categories.Count, result.Count());
    }

    [Fact]
    public async Task GetCategoryById_ValidId_ShouldReturnCategory()
    {
        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object);

        var existingCategory = new Category("Action");
        mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingCategory);

        var result = await categoryService.GetCategoryById(1);

        Assert.NotNull(result);
        Assert.Equal(existingCategory.Id, result.Id);
    }

    [Fact]
    public async Task GetCategoryById_InvalidId_ShouldThrowInvalidOperationException()
    {
        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object);

        # pragma warning disable CS8620, CS8600
        mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Category)null);
        await Assert.ThrowsAsync<InvalidOperationException>(() => categoryService.GetCategoryById(1));
    }

    [Fact]
    public async Task UpdateCategory_SuccessfulUpdate_ShouldUpdateCategory()
    {
        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object);

        var existingCategory = new Category("Action");
        mockRepository.Setup(repo => repo.GetByIdAsync(existingCategory.Id)).ReturnsAsync(existingCategory);

        var updatedCategory = new Category("Updated");
        await categoryService.UpdateCategory(updatedCategory);

        mockRepository.Verify(repo => repo.UpdateAsync(updatedCategory), Times.Once);
    }

    [Fact]
    public async Task UpdateCategory_InvalidData_ShouldThrowValidationException()
    {
        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object);

        var invalidCategory = new Category("");

        await Assert.ThrowsAsync<ValidationException>(() => categoryService.UpdateCategory(invalidCategory));
        mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task UpdateCategory_NonExistingCategory_ShouldThrowInvalidOperationException()
    {
        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object);

        # pragma warning disable CS8620, CS8600
        mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null);
        var nonExistingCategory = new Category("NonExisting");

        await Assert.ThrowsAsync<InvalidOperationException>(() => categoryService.UpdateCategory(nonExistingCategory));
        mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task DeleteCategory_SuccessfulDeletion_ShouldDeleteCategory()
    {
        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object);

        var existingCategory = new Category("Action");
        mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingCategory);

        await categoryService.DeleteCategory(1);
        mockRepository.Verify(repo => repo.DeleteAsync(existingCategory), Times.Once);
    }

    [Fact]
    public async Task DeleteCategory_InvalidId_ShouldThrowInvalidOperationException()
    {
        var mockRepository = new Mock<ICategoryRepository>();
        var categoryService = new CategoryService(mockRepository.Object);

        mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Category)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => categoryService.DeleteCategory(1));
        mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Category>()), Times.Never);
    }
}