using FluentValidation;
using OpenMovies.Models;
using OpenMovies.Repositories;
using OpenMovies.Validators;

namespace OpenMovies.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task CreateCategory(Category category)
    {
        var validation = new CategoryValidation();
        var validationResult = await validation.ValidateAsync(category);

        if (!validationResult.IsValid)
            throw new ValidationException("Validation failed.", validationResult.Errors);

        var existingCategory = await _categoryRepository.GetAsync(c => c.Name == category.Name);
        if (existingCategory != null)
            throw new InvalidOperationException("A category with the same name already exists.");

        await _categoryRepository.AddAsync(category);
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<Category> GetCategoryById(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new InvalidOperationException($"Category with ID {id} not found.");

        return category;
    }

    public async Task UpdateCategory(Category category)
    {
        var validation = new CategoryValidation();
        var validationResult = await validation.ValidateAsync(category);

        if (!validationResult.IsValid)
            throw new ValidationException("Validation failed.", validationResult.Errors);

        var existingCategory = await _categoryRepository.GetByIdAsync(category.Id);
        if (existingCategory == null)
            throw new InvalidOperationException("Category not found.");

        await _categoryRepository.UpdateAsync(category);
    }

    public async Task DeleteCategory(int categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
            throw new InvalidOperationException("Category not found.");

        await _categoryRepository.DeleteAsync(category);
    }
}
