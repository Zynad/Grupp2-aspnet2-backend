using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos;

public class CategoryDTO
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;

	public static implicit operator CategoryDTO(CategoryEntity entity)
	{
		return new CategoryDTO
		{
			Id = entity.Id,
			Name = entity.Name
		};
	}
}
