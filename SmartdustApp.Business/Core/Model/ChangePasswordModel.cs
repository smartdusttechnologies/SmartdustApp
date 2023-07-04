using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
        public class ChangePasswordModel : Entity
        {

            public string OldPassword { get; set; }


            public string NewPassword { get; set; }


            public string ConfirmPassword { get; set; }
            public string Username { get; set; }

            // public string User { get; set; }

            public int UserId { get; set; }
            public string PasswordHash { get; set; }
            public string PasswordSalt { get; set; }
        }
    }
