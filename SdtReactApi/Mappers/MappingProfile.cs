using AutoMapper;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.DTO;
using SmartdustApp.Web.Models;

namespace SmartdustApp.Web.UI.Mappers
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDTO,UserModel>().ReverseMap();
            CreateMap<LeaveDTO,LeaveModel>().ReverseMap();
            CreateMap<UpdateLeaveStatusDTO, UpdateLeaveModel>().ReverseMap();
            CreateMap<LeaveBalanceDTO, LeaveBalance>().ReverseMap();
            CreateMap<EmployeeDTO, EmployeeTable>().ReverseMap();
            CreateMap<ContactDTO, ContactModel>().ReverseMap();
            CreateMap<ChangePasswordDTO, ChangePasswordModel>().ReverseMap();
        }
    }
}