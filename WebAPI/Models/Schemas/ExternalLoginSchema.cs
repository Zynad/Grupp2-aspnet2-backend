using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WebAPI.Models.Schemas
{
	public class ExternalLoginSchema
	{
		public string LoginProvider { get; set; } = null!;
		public string ProviderKey { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Email { get; set; } = null!;

		public static implicit operator UserLoginInfo(ExternalLoginSchema externalLoginSchema)
		{
			return new UserLoginInfo
			(
				externalLoginSchema.LoginProvider,
				externalLoginSchema.ProviderKey,
				externalLoginSchema.LoginProvider
				
			);
		}
	}
}
