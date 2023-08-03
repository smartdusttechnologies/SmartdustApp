using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Enums
{
    public class LookupMapping
    {
        public static Dictionary<Lookup, int> TypeToID = new Dictionary<Lookup, int>
        {
        { Lookup.Medical, 1 },
        { Lookup.Paid, 2 },
        { Lookup.LeaveOfAbsence, 3 },
        { Lookup.Approve, 5 },
        { Lookup.Pending, 6 },
        { Lookup.Decline, 7 }
        };
    }
}
