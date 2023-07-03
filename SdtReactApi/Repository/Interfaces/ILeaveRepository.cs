using SmartdustApi.DTO;

namespace SmartdustApi.Repository.Interface
{
    public interface ILeaveRepository
    {
        List<LeaveDTO> Get();
        int Insert(LeaveDTO entity);
        int StatusUpdate(LeaveDTO entity);
        List<LeaveDTO> GetByUser(int id);

        
    }
}