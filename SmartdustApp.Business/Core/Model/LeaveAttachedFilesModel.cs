using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    // LeaveAttachedFilesModel class represents a model for associating attached files with a leave request.
    public class LeaveAttachedFilesModel
    {
        // Identifier for the leave request to which files are attached.
        public int LeaveID { get; set; }

        // Identifier for the attached document/file.
        public int DocumentID { get; set; }
    }

}
