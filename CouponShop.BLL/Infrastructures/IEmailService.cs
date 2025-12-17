using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponShop.BLL.Infrastructures
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body);

    }
}
