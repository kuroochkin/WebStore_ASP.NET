using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Interfaces.Services
{
	public interface IUsersClient : 
		IUserRoleStore<User>,
		IUserPasswordStore<User>,
		IUserEmailStore<User>,
		IUserPhoneNumberStore<User>,
		IUserTwoFactorStore<User>,
		IUserLoginStore<User>,
		IUserClaimStore<User>
	{
	}

}
