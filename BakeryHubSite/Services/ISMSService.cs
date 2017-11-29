using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryHub.Services
{
    public interface ISMSService
    {
        Task SendAsync(string phoneNumber, string text);
    }
}
