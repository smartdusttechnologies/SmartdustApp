using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Interfaces
{
    public interface ILeaveService
    {
        RequestResult<List<LeaveModel>> Get();
        RequestResult<bool> Save(LeaveModel leave);
    }
}
