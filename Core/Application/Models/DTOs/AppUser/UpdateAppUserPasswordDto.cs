using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.AppUser
{
    public class UpdateAppUserPasswordDto
    {
        public string Id { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
