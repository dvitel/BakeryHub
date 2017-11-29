using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryHub.Services
{
    public class SMSService : ISMSService
    {
        public Task SendAsync(string phoneNumber, string text)
        {
            throw new NotImplementedException();
        }
    }
}
