using WebAPI.Models.Dtos;
using WebAPI.Models.Schemas;

namespace WebAPI.Models.Interfaces
{
    public interface ITagService
    {
        Task<bool> CreateAsync(TagSchema schema);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<TagDTO>> GetAllAsync();
        Task<TagDTO> GetByIdAsync(Guid id);
    }
}