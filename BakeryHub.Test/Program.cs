using BakeryHub.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BakeryHub.Test
{

    class Program
    {
        static public async Task TestEmail()
        {
            try
            {
                var builder = new ConfigurationBuilder();
                builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "EmailCredentials:Login", "???" },
                { "EmailCredentials:Password", "???" }
            });
                var config = builder.Build();
                var emailService = new GmailEmailService(config);
                await emailService.SendAsync("???", "Test message", "Some text");
                Console.WriteLine("Done!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadKey();
        }

        static public async Task TestSMS()
        {
            try
            {
                var builder = new ConfigurationBuilder();
                builder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "SMSCredentials:KeyId", "???" },
                { "SMSCredentials:SecretKey", "???" }
            });
                var config = builder.Build();
                var emailService = new AmazonSNSService(config);
                await emailService.SendAsync("???", "Test from bakeryHub");
                Console.WriteLine("Done!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }            
        }

        static async Task Main(string[] args)
        {
            await TestSMS();
            Console.ReadKey();
        }
    }
}
