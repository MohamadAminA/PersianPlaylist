using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public long Mobile { get; set; }
        public int VerificationCode { get; set; }
        public DateTime ExpireVerificationCode { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
