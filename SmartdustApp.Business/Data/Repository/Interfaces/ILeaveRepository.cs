using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Data.Repository.Interfaces
{
    public interface ILeaveRepository
    {
        List<LeaveModel> Get();
    }
}
