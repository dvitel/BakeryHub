using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryHub.Services
{
    public class AmazonSNSService : ISMSService
    {
        IConfiguration config;
        public AmazonSNSService(IConfiguration configu) => config = configu;
        public async Task SendAsync(string phoneNumber, string text)
        {
            var client = new AmazonSimpleNotificationServiceClient(
                awsAccessKeyId: config["SMSCredentials:KeyId"],
                awsSecretAccessKey: config["SMSCredentials:SecretKey"],                
                region: RegionEndpoint.USEast1
                );
            await client.PublishAsync(new PublishRequest { PhoneNumber = phoneNumber, Message = text });
        }
    }
}
