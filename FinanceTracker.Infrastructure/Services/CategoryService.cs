using AutoMapper;
using FinanceTracker.Application.DTOs.Category;
using FinanceTracker.Application.Exceptions;
using FinanceTracker.Application.Interfaces.Repositories;
using FinanceTracker.Application.Interfaces.Services;
using FinanceTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task AddCategoryAsync(int userId, CreateCategoryRequestDto category)
        {
            Category newCategory = _mapper.Map<Category>(category);
            newCategory.UserId = userId;
            await _categoryRepository.AddCategoryAsync(newCategory);
        }

        public async Task DeleteCategoryAsync(int id, int userId)
        {
            Category? categoryToDelete = await _categoryRepository.GetCategoryByIdAsync(id);
            if (categoryToDelete == null || categoryToDelete.UserId != userId)
            {
                throw new AppException("CATEGORY_NOT_FOUND_OR_BELONGS_TO_OTHER_USER", "კატეგორია არ არსებობს ან სხვა იუზერს ეკუთვნის", 404);
            }
            if (await _categoryRepository.HasTransactionsAsync(categoryToDelete.Id))
            {
                throw new AppException("CATEGORY_HAS_TRANSACTIONS", "კატეგორიას აქვს ტრანზაქციები, ამიტომ ვერ წაიშლება", 409);
            }
            await _categoryRepository.DeleteCategoryAsync(categoryToDelete);
        }

        public async Task<List<CategoryResponseDto>> GetAllCategoriesByUserId(int userId)
        {
            var categories = await _categoryRepository.GetAllCategoriesByUserId(userId);
            return categories.Select(c => _mapper.Map<CategoryResponseDto>(c)).ToList();
        }

        public async Task UpdateCategoryAsync(int id, int userId, UpdateCategoryRequestDto category)
        {
            Category? categoryToUpdate = await _categoryRepository.GetCategoryByIdAsync(id);
            if (categoryToUpdate == null || categoryToUpdate.UserId != userId)
            {
                throw new AppException("CATEGORY_NOT_FOUND_OR_BELONGS_TO_OTHER_USER", "კატეგორია არ არსებობს ან სხვა იუზერს ეკუთვნის", 404);
            }
            _mapper.Map(category, categoryToUpdate);
            await _categoryRepository.UpdateCategoryAsync(categoryToUpdate);
        }
    }
}
