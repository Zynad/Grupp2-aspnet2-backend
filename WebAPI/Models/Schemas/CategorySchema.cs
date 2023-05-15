using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
	public class CategorySchema
	{
		public string Name { get; set; } = null!;

		public static implicit operator CategoryEntity(CategorySchema schema)
		{
			return new CategoryEntity
			{
				Id = Guid.NewGuid(),
				Name = schema.Name
			};
		}
	}
}
