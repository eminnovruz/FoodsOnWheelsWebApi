using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.Auth
{
    public class RefreshTokenDto
    {
        public string AppUserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpireDate { get; set; }
    }
}
