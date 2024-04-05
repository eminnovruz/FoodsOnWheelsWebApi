using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Common;

namespace Domain.Models
{
    public class AppUser : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PassHash { get; set; }
        public byte[] PassSalt { get; set; }
        public string Role { get; set; }
        public string RefreshToken { get; set; } 
        public DateTime TokenExpireDate { get; set; }
    }
}
