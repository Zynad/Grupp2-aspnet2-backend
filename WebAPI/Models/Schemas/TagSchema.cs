using Microsoft.Build.Framework;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
	public class TagSchema
	{
		[Required]
		public string Name { get; set; } = null!;

		public static implicit operator TagEntity(TagSchema schema)
		{
			return new TagEntity
			{
				Id = Guid.NewGuid(),
				Name = schema.Name
			};
		}
	}
}
