﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    public class LeaveBalance
    {
        public int UserID { get; set; }
        public string LeaveType { get; set; }
        public int Available { get; set; }
    }
}