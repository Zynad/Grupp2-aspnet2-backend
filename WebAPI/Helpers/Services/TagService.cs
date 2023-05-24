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
			try
			{
				var tags = await _tagRepo.GetAllAsync();
				var dtos = new List<TagDTO>();

				foreach (var entity in tags)
				{
					dtos.Add(entity);
				}

				return dtos;
			}
			catch { }
			return null!;
		}

		public async Task<TagDTO> GetByIdAsync(Guid id)
		{
			try
			{
				var tag = await _tagRepo.GetAsync(x => x.Id == id);
				TagDTO dto = tag;

				return dto;
			}
			catch { }
			return null!;
		}

		public async Task<bool> CreateAsync(TagSchema schema)
		{
			try
			{
				TagEntity entity = schema;
				await _tagRepo.AddAsync(entity);

				return true;
			}
			catch { }
			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			try
			{
				var entity = await _tagRepo.GetAsync(x => x.Id == id);
				await _tagRepo.DeleteAsync(entity!);

				return true;
			}
			catch { }
			return false;
		}
	}
}

