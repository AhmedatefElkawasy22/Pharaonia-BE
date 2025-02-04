using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharaonia.Domain.DTOs
{
    public class LoginDTO
    {
        [DataType(DataType.EmailAddress),Required]
        public string Email { get; set; }
        [DataType(DataType.Password), Required]
        public string Password { get; set; }
    }
}
