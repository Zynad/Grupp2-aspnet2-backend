using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services
{
	public class TagService
	{
		private readonly TagRepo _tagRepo;

		public TagService(TagRepo tagRepo)
		{
			_tagRepo = tagRepo;
		}

		public async Task<IEnumerable<TagDTO>> GetAllAsync()
		{
			var tags = await _tagRepo.GetAllAsync();

			var dtos = new List<TagDTO>();

			foreach (var entity in tags)
			{
				dtos.Add(entity);
			}

			return dtos;
		}

		public async Task<TagDTO> GetByIdAsync(Guid id)
		{
			var tag = await _tagRepo.GetAsync(x => x.Id == id);

			TagDTO dto = tag;

			return dto;
		}

		public async Task<bool> CreateAsync(TagSchema schema)
		{
			TagEntity entity = schema;

			try
			{
				await _tagRepo.AddAsync(entity);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var entity = await _tagRepo.GetAsync(x => x.Id == id);

			try
			{
				await _tagRepo.DeleteAsync(entity!);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}

