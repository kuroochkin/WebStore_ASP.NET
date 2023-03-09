using System.Security.Claims;

namespace WebStore.Domain.DTO.Identity
{
	public abstract class ClaimDTO : UserDTO
	{
		public IEnumerable<Claim> Claims { get; set; }
	}

	public class AddClaimDTO : ClaimDTO { }

	public class RemoveClaimDTO : ClaimDTO { }

	public class RemplaceClaimDTO : UserDTO
	{
		public Claim Claim { get; set; }
		public Claim NewClaim { get; set; }
	}


}
