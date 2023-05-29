using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Interfaces;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services
{
    public class TagService : ITagService
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

        public async Task<bool> CheckOrCreateAsync(List<string> tags)
        {
            try 
            {
                foreach (var tag in tags)
                {
                    var result = await _tagRepo.GetAsync(x => x.Name == tag);
                    if (result == null)
                    {
                        var tagEntity = new TagEntity
                        {
                            Id = Guid.NewGuid(),
                            Name = tag
                        };
                        await _tagRepo.AddAsync(tagEntity);
                    }
                }
                return true;
            }
            catch { }
            return false;
        }
    }
}

