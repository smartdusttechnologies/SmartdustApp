using AutoMapper;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Web.Models;

namespace SmartdustApp.Web.UI.Mappers
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
           CreateMap<UserDTO,UserModel>().ReverseMap();
        }
    }
}