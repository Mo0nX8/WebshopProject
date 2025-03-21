using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.EntityFramework.Data;

namespace Webshop.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendOrderEmailAsync(Orders order, string email);
        Task SendResetEmailAsync(string email, string subject, string body);
    }
}
