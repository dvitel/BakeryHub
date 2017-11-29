using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryHub.Services
{
    public interface IEmailService
    {
        Task SendAsync(string email, string subject, string text);
    }
}
