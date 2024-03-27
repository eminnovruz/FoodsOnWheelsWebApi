using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Common
{
    public class AppUser : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PassHash { get; set; }
        public byte[] PassSalt { get; set; }
    }
}
