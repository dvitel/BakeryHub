using BakeryHub.Domain;
using BakeryHub.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private static Random rand = new Random();

        public int randInt(int start, int end)
        {
            return rand.Next(end - start + 1) + start;
        }

        public DateTime randDateTime(DateTime start, DateTime end)
        {
            var totalSec = (int)(end - start).TotalSeconds;
            return start.AddSeconds(rand.Next(totalSec));
        }
        public DateTime randDateTime(DateTime start)
        {
            return randDateTime(start, DateTime.UtcNow);
        }
        public DateTime randDateTime(int days)
        {
            var end = DateTime.UtcNow;
            var start = end.AddDays(-days);
            var totalSec = (int)(end - start).TotalSeconds;
            return start.AddSeconds(rand.Next(totalSec));
        }
        public T randSet<T>(IEnumerable<T> lst)
        {
            var l = lst.ToList();
            return l[rand.Next(0, l.Count)];
        }

        public IEnumerable<T> randN<T>(int N, Func<int, T> produce)
        {
            var i = 0;
            while (i < rand.Next(N))
            {
                yield return produce(i);
                i++;
            }
        }

        public bool randBool()
        {
            return rand.NextDouble() > 0.5;
        }

        public void GenerateSuppliers(int N)
        {
            var genId = Guid.NewGuid();
            var optionsBuilder = new DbContextOptionsBuilder<BakeryHubContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BakeryHub;Trusted_Connection=True;MultipleActiveResultSets=true");
            var context = new BakeryHubContext(optionsBuilder.Options);
            for (var i = 0; i < N; i++)
            {
                var firstVisit = randDateTime(-10);
                var supplier =
                    new Supplier {
                        Name = $"Bakery #{i}, {genId}",
                        Description = "Some simple description",
                        HasLogo = false,
                        IsCompany = randBool(),
                        IsApproved = randBool(),
                        Products = 
                            randN(5, j => new Product
                            {
                                ProductId = j,
                                Name = $"Product #{j}",
                                Description = "Best product",
                                Price = (decimal)(rand.NextDouble() * 100 + 1),
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = rand.NextDouble() > 0.8 ? randInt(1,5) : 0,
                                CategoryId = randInt(3,4),
                                Images = 
                                    rand.NextDouble() > 0.7 ? 
                                        randN(3, k => new ProductImage
                                        {
                                            ImageId = k,
                                            Path = "",
                                            LogicalPath = "",
                                            Mime = "",                                            
                                        }).ToList() : null
                            }).ToList(),
                        User = 
                            new User
                            {
                                Login = $"gen_user_{genId}_{i}",
                                Password = $"gen_pwd_{i}",
                                PasswordEncryptionAlgorithm = User.PasswordEncryption.Plain,
                                Salt = "",
                                Session = new Session
                                {
                                    IP = $"127.0.0.{randInt(1, 253)}",
                                    FirstVisit = firstVisit,
                                    LastVisit = randDateTime(firstVisit),
                                    UserAgent =
                                        randSet(
                                            new[] {
                                                "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko",
                                                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36",
                                                "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0",
                                                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41",
                                                "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_1 like Mac OS X) AppleWebKit/603.1.30 (KHTML, like Gecko) Version/10.0 Mobile/14E304 Safari/602.1",
                                                "Googlebot/2.1 (+http://www.google.com/bot.html)",
                                                "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)"
                                            }),
                                },
                                Addresses = 
                                    randN(3, j => new Address
                                    {
                                        AddressId = j,
                                        City = randSet(new[] {
                                            "City 1",
                                            "City 2",
                                            "City 3",
                                        }),
                                        CountryId = "US",
                                        StateId = randSet(new []
                                        {
                                            "FL", "WA"
                                        }),
                                        IsBilling = false,
                                        IsDeleted = false,
                                        Street = randSet(new[] {
                                            "Street 1",
                                            "Street 2",
                                            "Street 3",
                                        }),
                                        Zip = "123456"
                                    }).ToList(),
                                Contacts = randN(4, j => {
                                    var t = 
                                        randSet(new[] { Contact.ContactType.Email,
                                            Contact.ContactType.Mobile, Contact.ContactType.Phone });
                                    return new Contact
                                    {
                                        ContactId = j,
                                        Type = t,
                                        Address =
                                            t == Contact.ContactType.Email ?
                                                $"supplier{i}_{j}@domain.com" :
                                                "123456789",
                                        IsConfirmed = rand.NextDouble() < 0.9,
                                        IsDeleted = rand.NextDouble() < 0.9,
                                        IsPrivate = rand.NextDouble() < 0.9,
                                        Name = $"Person #{j} of supplier {j}",
                                        ReportSubscriptions =
                                            t == Contact.ContactType.Email ?
                                                new List<ReportSubscription>()
                                                {
                                                    //;l
                                                } : null
                                    };
                                }).ToList()
                            }
                    };
            }
        }

        static async Task Main(string[] args)
        {
            await TestSMS();
            Console.ReadKey();
        }
    }
}
