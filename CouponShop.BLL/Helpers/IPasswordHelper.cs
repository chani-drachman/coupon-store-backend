using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Helpers
{
    public interface IPasswordHelper
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string storedHash);
    }
}
