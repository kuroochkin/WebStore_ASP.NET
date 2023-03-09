using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Identity
{
	public class RolesClient : BaseClient
	{
		public RolesClient(HttpClient Client) : base(Client, WebApiAddresses.Identity.Roles)
		{
		}
	}
}
