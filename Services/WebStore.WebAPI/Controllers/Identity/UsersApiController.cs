using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers.Identity
{
	[ApiController]
	[Route(WebApiAddresses.Identity.Users)]
	public class UsersApiController : ControllerBase
	{
		
	}

	[ApiController]
	[Route(WebApiAddresses.Identity.Roles)]
	public class RolesApiController : ControllerBase
	{

	}
}
