using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos
{
	public class TagDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;

		public static implicit operator TagDTO(TagEntity entity)
		{
			return new TagDTO
			{
				Id = entity.Id,
				Name = entity.Name
			};
		}
	}
}
