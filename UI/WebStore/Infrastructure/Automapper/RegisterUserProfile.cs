using AutoMapper;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels.Identity;

namespace WebStore.Infrastructure.Automapper
{
	public class RegisterUserProfile : Profile
	{
		public RegisterUserProfile()
		{
			CreateMap<RegisterUserViewModel, User>()
				.ForMember(user => user.UserName, opt => opt.MapFrom(model => model.UserName))
				.ReverseMap();	
		}
	}
}
