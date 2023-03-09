using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers.Identity
{
	[ApiController]
	[Route(WebApiAddresses.Identity.Users)]
	public class UsersApiController : ControllerBase
	{
		private readonly UserStore<User, Role, WebStoreDB> _UserStore;

		public UsersApiController(WebStoreDB db)
		{
			_UserStore = new UserStore<User, Role, WebStoreDB>(db);

		}

		[HttpGet("all")]
		public async Task<IEnumerable<User>> GetAll() => await _UserStore.Users.ToArrayAsync();


	}
}
