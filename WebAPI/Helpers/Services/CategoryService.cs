using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services
{
	public class CategoryService
	{
		private readonly CategoryRepo _categoryRepo;

		public CategoryService(CategoryRepo categoryRepo)
		{
			_categoryRepo = categoryRepo;
		}

		public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
		{
			var categories = await _categoryRepo.GetAllAsync();

			var dtos = new List<CategoryDTO>();

			foreach (var entity in categories)
			{
				dtos.Add(entity);
			}

			return dtos;
		}

		public async Task<CategoryDTO> GetByIdAsync(Guid id)
		{
			var category = await _categoryRepo.GetAsync(x => x.Id == id);

			CategoryDTO dto = category;

			return dto;
		}

		public async Task<bool> CreateAsync(CategorySchema schema)
		{
			CategoryEntity entity = schema;

			try
			{
				await _categoryRepo.AddAsync(entity);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var entity = await _categoryRepo.GetAsync(x => x.Id == id);

			try
			{
				await _categoryRepo.DeleteAsync(entity!);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
