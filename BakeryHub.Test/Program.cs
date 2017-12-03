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
        static public async Task TestEmail(IConfiguration config)
        {
            try
            {                
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

        static public async Task TestSMS(IConfiguration config)
        {
            try
            {
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

        public static int randInt(int start, int end)
        {
            return rand.Next(end - start + 1) + start;
        }

        public static DateTime randDateTime(DateTime start, DateTime end)
        {
            var totalSec = (int)(end - start).TotalSeconds;
            return start.AddSeconds(rand.Next(totalSec));
        }
        public static DateTime randDateTime(DateTime start)
        {
            return randDateTime(start, DateTime.UtcNow);
        }
        public static DateTime randDateTime(int days, DateTime? now = null)
        {
            var end = now ?? DateTime.UtcNow;
            var start = end.AddDays(-days);
            var totalSec = (int)(end - start).TotalSeconds;
            return start.AddSeconds(rand.Next(totalSec));
        }
        public static T randSet<T>(IEnumerable<T> lst)
        {
            var l = lst.ToList();
            return l[rand.Next(0, l.Count)];
        }

        public static IEnumerable<T> randN<T>(int N, Func<int, T> produce, int start = 0, int min = 0)
        {
            var i = start;
            var max = rand.Next(N) + min + start;
            while (i < max)
            {
                yield return produce(i);
                i++;
            }
        }

        public static bool randBool(double threshold = 0.5)
        {
            return rand.NextDouble() > threshold;
        }

        public static IEnumerable<T> genN<T>(int N, Func<int, T> gen, int start=0)
        {
            for (int i = start; i < N; i++)
            {
                yield return gen(i);
            }
        }

        public static async Task CreateSupplier(IConfiguration config, string env)
        {
            var genId = Guid.NewGuid();
            var optionsBuilder = new DbContextOptionsBuilder<BakeryHubContext>();
            optionsBuilder.UseSqlServer(config[$"ConnectionString:{env}"])
                .EnableSensitiveDataLogging();            
            var firstVisit = randDateTime(10);
            var supplier =
                new Supplier {
                    Name = $"European pastry in Florida",
                    Description =
                        @"We are most often ordered:\r\n
- The Honey cake;\r\n
- Yeast bakery (pies and patties)\r\n
- Napoleon \r\n
- Prague cake \r\n
- Bird's Milk (cake) \r\n
- French caramel flan (cake) \r\n
- Fruit cake-souffle \r\n
- Puff tube \r\n
- Eclairs (custard cakes).",
                    HasLogo = true,
                    IsCompany = true,
                    IsApproved = true,
                    Products =
                        new List<Product>()
                        {
                            new Product
                            {
                                ProductId = 1,
                                Name = $"Honey cake with customized decoration",
                                Description = "Fondant decoration",
                                Price = 35,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    new List<ProductImage>
                                    {
                                        new ProductImage
                                        {
                                            ImageId = 0,
                                            LogicalPath = "/products/1_1_0.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }
                                    }
                            },
                            new Product
                            {
                                ProductId = 2,
                                Name = $"Honey cake",
                                Description = "Made of honey",
                                Price = 28,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(5, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_2_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 3,
                                Name = $"Two level honey cake",
                                Description = "Base level is x2 size, weight 7lb",
                                Price = 62,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(2, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_3_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 4,
                                Name = $"Strawberry decorated honey cake",
                                Description = "Base size x1 is 3.5lb",
                                Price = 31,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(6, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_4_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 5,
                                Name = $"x2 Honey cake",
                                Description = "7lb, no decore",
                                Price = 56,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_5_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 6,
                                Name = $"2 level honey cake",
                                Description = "7lb, no decore",
                                Price = 62,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_6_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 7,
                                Name = $"Honey cake for baby shower",
                                Description = "x1 (3.5lb) + decore",
                                Price = 33,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_7_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 8,
                                Name = $"Wedding loaf",
                                Description = "Carawai, Russion traditional",
                                Price = 30,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 2,
                                Images =
                                    genN(2, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_8_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 9,
                                Name = $"Bird milk cake",
                                Description = "Suffle cake",
                                Price = 28,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(3, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_9_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 10,
                                Name = $"Honey cake with flower decorated fondant",
                                Description = "Suffle cake",
                                Price = 31,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_10_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 11,
                                Name = $"Fish pie",
                                Description = "Coulibiaka, Russion traditional",
                                Price = 35,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 2,
                                Images =
                                    genN(3, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_11_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 12,
                                Name = $"Pirogi with apples",
                                Description = "Russion traditional, x12 in pack",
                                Price = 20,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 3,
                                Images =
                                    genN(2, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_12_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 13,
                                Name = $"Pirogi with cabbage",
                                Description = "Russion traditional, x12 in pack",
                                Price = 20,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 3,
                                CategoryId = 3,
                                Images =
                                    genN(3, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_13_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 14,
                                Name = $"Chocolate cake",
                                Description = "3.5lb",
                                Price = 28,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(2, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_14_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 15,
                                Name = $"Honey cake again",
                                Description = "3.5lb",
                                Price = 28,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_15_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 16,
                                Name = $"Honey cake under simple fondant decoration",
                                Description = "3.5lb",
                                Price = 31,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_16_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 17,
                                Name = $"Prague cake",
                                Description = "3.5lb",
                                Price = 28,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_17_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 18,
                                Name = $"Honey cake under customized decoration",
                                Description = "3.5lb",
                                Price = 31,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_18_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 19,
                                Name = $"Wafle cake under nuts",
                                Description = "3.5lb",
                                Price = 25,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_19_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 20,
                                Name = $"Honey cake wiht bee cells",
                                Description = "3.5lb",
                                Price = 29,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_20_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 21,
                                Name = $"Chocolate cake",
                                Description = "3.5lb",
                                Price = 28,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(2, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_21_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 22,
                                Name = $"Cookies",
                                Description = "x10 per pack",
                                Price = 20,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 2,
                                CategoryId = 3,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_22_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 23,
                                Name = $"Small honey cake",
                                Description = "x0.5, 1.8lb, available only with ordering other cakes",
                                Price = 14,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_23_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 24,
                                Name = $"Fish pie, 0.5",
                                Description = "Rusian traditional",
                                Price = 15,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 2,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_24_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 25,
                                Name = $"Poppy seeds cookies",
                                Description = "Rusian traditional, x8 per pack",
                                Price = 15,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 1,
                                CategoryId = 3,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_25_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 26,
                                Name = $"Chicken pie",
                                Description = "Kurnick, Rusian traditional",
                                Price = 30,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 2,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_26_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 27,
                                Name = $"Honey cake, business style",
                                Description = "Fondant decoration",
                                Price = 33,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_27_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 28,
                                Name = $"Honey cake, bee hive, x0.5",
                                Description = "Available only for ordering with other cakes",
                                Price = 24,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_28_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 29,
                                Name = $"Honey cake with cocoa fondant",
                                Description = "3.5lb",
                                Price = 31,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_29_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 30,
                                Name = $"Honey cake, cow farm",
                                Description = "3.5lb",
                                Price = 33,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_30_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 31,
                                Name = $"Honey volcano",
                                Description = "3 levels, x3, 10lb",
                                Price = 89,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_31_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 32,
                                Name = $"Honey cake, business style, 2 levels",
                                Description = "x2, 7lb",
                                Price = 64,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_32_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 33,
                                Name = $"Spartak cake under chocolate decore",
                                Description = "x1, 3.5lb",
                                Price = 35,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_33_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 34,
                                Name = $"Potato cookies",
                                Description = "x12 per pack",
                                Price = 18,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 3,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_34_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 35,
                                Name = $"Spartak cake with messages",
                                Description = "x1, 3.5lb",
                                Price = 28,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_35_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 36,
                                Name = $"Chocolate cherry cake with decore",
                                Description = "x1, 3.5lb",
                                Price = 28,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_36_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 37,
                                Name = $"Honey cake with business decoration, 2 levels",
                                Description = "x1.5, 5.5lb",
                                Price = 45,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_37_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 38,
                                Name = $"Honey cake, 3 levels",
                                Description = "x3, 10lb",
                                Price = 83,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(2, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_38_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 39,
                                Name = $"Fruit cake",
                                Description = "2lb",
                                Price = 23,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 0,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_39_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 40,
                                Name = $"Pipes (horns) with cream",
                                Description = "x12 per pack",
                                Price = 20,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 1,
                                CategoryId = 3,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_40_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 41,
                                Name = $"Pufs with cream",
                                Description = "x12 per pack",
                                Price = 24,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 3,
                                CategoryId = 3,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_41_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                            new Product
                            {
                                ProductId = 42,
                                Name = $"Milk cream pie",
                                Description = "2lb",
                                Price = 27,
                                LastUpdated = DateTime.UtcNow,
                                AvailableNow = 3,
                                CategoryId = 1,
                                Images =
                                    genN(1, i =>
                                        new ProductImage
                                        {
                                            ImageId = i,
                                            LogicalPath = $"/products/1_42_{i}.jpg",
                                            Path = "",
                                            Mime = "image/jpg",
                                        }).ToList()
                            },
                        },
                    User = 
                        new User
                        {
                            Login = config["cred:login"],
                            Password = config["cred:pwd"],
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
                                new List<Address> {
                                    new Address
                                    {
                                        AddressId = 1,
                                        City = "Sait Petersburg",
                                        CountryId = "US",
                                        StateId = "FL",
                                        IsBilling = false,
                                        IsDeleted = false,
                                        Street = config["supplier_address"],
                                        Zip = ""
                                    }
                                },
                            Contacts = 
                                new List<Contact>
                                {
                                    new Contact
                                    {
                                        ContactId = 1,
                                        Type = Contact.ContactType.Email,
                                        Address = config["dev_contact_email"],
                                        IsConfirmed = true,
                                        IsDeleted = false,
                                        IsPrivate = true,
                                        Name = $"Developer 1",
                                        ReportSubscriptions =
                                            new List<ReportSubscription>()
                                            {
                                                new ReportSubscription
                                                {
                                                    Type = ReportSubscription.ReportType.OrderAvailable                                                    
                                                },
                                                new ReportSubscription
                                                {
                                                    Type = ReportSubscription.ReportType.MonthlySales
                                                },
                                                new ReportSubscription
                                                {
                                                    Type = ReportSubscription.ReportType.Feedback
                                                }
                                            }
                                    },
                                    new Contact
                                    {
                                        ContactId = 2,
                                        Type = Contact.ContactType.Mobile,
                                        Address = config["dev_contact_sms"],
                                        IsConfirmed = true,
                                        IsDeleted = false,
                                        IsPrivate = true,
                                        Name = $"Developer 1",
                                        ReportSubscriptions =
                                            new List<ReportSubscription>()
                                            {
                                                new ReportSubscription
                                                {
                                                    Type = ReportSubscription.ReportType.OrderAvailable
                                                }
                                            }
                                    },
                                    new Contact
                                    {
                                        ContactId = 3,
                                        Type = Contact.ContactType.Phone,
                                        Address = config["supplier_phone"],
                                        IsConfirmed = true,
                                        IsDeleted = false,
                                        IsPrivate = false,
                                        Name = $"Joe, 8am-8pm"
                                    }
                                }
                        }
                };
            using (var context = new BakeryHubContext(optionsBuilder.Options))
            {
                await context.Suppliers.AddAsync(supplier);
                await context.SaveChangesAsync();
            }
        }

        public static async Task CreateCustomers(IConfiguration config, string[] names, string env)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BakeryHubContext>();
            optionsBuilder.UseSqlServer(config[$"ConnectionString:{env}"])
                .EnableSensitiveDataLogging();
            var firstVisit = randDateTime(10);
            using (var context = new BakeryHubContext(optionsBuilder.Options))
            {
                var supplier = await (from s in context.Suppliers where s.Name == "European pastry in Florida" select s).FirstOrDefaultAsync();
                var customers =
                names.Select((name, i) =>
                    new Customer
                    {
                        Name = name,
                        User =
                            new User
                            {
                                Login = $"login_{i}",
                                Password = $"pwd_{i}",
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
                                    randN(3, j =>
                                        new Address
                                        {
                                            AddressId = j,
                                            City =
                                                randSet(new[]
                                                {
                                                    "Sait Petersburg",
                                                    "Clearwater",
                                                    "Tampa",
                                                    "Seminole"
                                                }),
                                            CountryId = "US",
                                            StateId = "FL",
                                            IsBilling = randBool(0.9),
                                            IsDeleted = randBool(0.9),
                                            Street = "Street ?",
                                            Zip = ""
                                        }, 1).ToList(),
                                Contacts =
                                    randN(3, j => {
                                        var contactType =
                                            randSet(new[]
                                            {
                                                 Contact.ContactType.Email,
                                                 Contact.ContactType.Mobile,
                                                 Contact.ContactType.Phone
                                            });
                                        return
                                            new Contact
                                            {
                                                ContactId = j,
                                                Type = contactType,
                                                Address =
                                                    contactType == Contact.ContactType.Email ?
                                                        $"email_{name}@domain{i}.com" :
                                                        $"+{i}0123456789"
                                                    ,
                                                IsConfirmed = true,
                                                IsDeleted = false,
                                                IsPrivate = true,
                                                Name = name
                                            };
                                    }, 1).ToList(),
                            },
                        Orders =
                            randN(6, j => {
                                var placed = randDateTime(100);
                                var plannedDelivery = placed.AddDays(randInt(4, 10));
                                return new Order {
                                    OrderId = j,
                                    SupplierId = supplier.Id,
                                    DatePlaced = randDateTime(100),
                                    LastUpdated = DateTime.UtcNow,
                                    PlannedDeliveryDate = plannedDelivery,
                                    Status =
                                        randSet((IEnumerable<Order.OrderStatus>)Enum.GetValues(typeof(Order.OrderStatus))),
                                    Price = (decimal)(100 * rand.NextDouble() + 5),
                                    OrderItems = 
                                        randN(5, k => new OrderItem
                                        {
                                            IsCanceled = randBool(0.9),
                                            ProductId = randInt(1,42),
                                            ProductCount = 
                                                randBool(0.8) ? randInt(1,3) : 1,
                                            TotalPrice = (decimal)(100 * rand.NextDouble() + 5),
                                        }, 0, 1).ToList(),                                    
                                };
                            }, 1, 1).ToList()
                    }
                );
            //next code is for removing generated duplicates in order items
            customers = 
                customers.Select(customer =>
                {
                    customer.Orders =
                        customer.Orders.Select(order =>
                            {
                                HashSet<int> productIds = new HashSet<int>();
                                order.OrderItems = 
                                    order.OrderItems.Where(item =>
                                    {
                                        var res = !productIds.Contains(item.ProductId);
                                        productIds.Add(item.ProductId);
                                        return res;
                                    }).ToList();
                                return order;
                            }
                        ).ToList();
                    return customer;
                });
                await context.Customers.AddRangeAsync(customers);
                await context.SaveChangesAsync();
            }
        }

        public static async Task FixOrderPrices(IConfiguration config, string env)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BakeryHubContext>();
            optionsBuilder.UseSqlServer(config[$"ConnectionString:{env}"])
                .EnableSensitiveDataLogging();
            using (var context = new BakeryHubContext(optionsBuilder.Options))
            {
                var orders = await (from order in context.Orders.Include(o => o.OrderItems) select order).ToListAsync();
                var products = await (from product in context.Products select product).ToDictionaryAsync(p => p.ProductId);
                orders =
                    orders.Select(order =>
                    {
                        order.OrderItems =
                            order.OrderItems.Select(item =>
                            {
                                item.TotalPrice = item.ProductCount * products[item.ProductId].Price;
                                return item;
                            }).ToList();
                        order.Price = order.OrderItems.Sum(item => item.TotalPrice);
                        return order;
                    }).ToList();
                context.Orders.UpdateRange(orders);
                await context.SaveChangesAsync();
            }
        }

        public static async Task CreateCartItems(IConfiguration config, string env)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BakeryHubContext>();
            optionsBuilder.UseSqlServer(config[$"ConnectionString:{env}"])
                .EnableSensitiveDataLogging();
            using (var context = new BakeryHubContext(optionsBuilder.Options))
            {
                var sessionIds = 
                    await (
                        from c in context.Customers
                        join u in context.Users on c.Id equals u.Id
                        join s in context.Session on u.SessionId equals s.Id
                        select s.Id).ToListAsync();
                var products =
                    (await (from product in context.Products select new { product.SupplierId, product.ProductId }).ToListAsync())
                    .Select(p => new Tuple<int, int>(p.SupplierId, p.ProductId)).ToHashSet();
                var carts =
                    sessionIds.SelectMany(sessionId =>
                        {
                            var selected = new HashSet<Tuple<int, int>>();
                            return
                                randN(3, i =>
                                    {
                                        var product = Tuple.Create(0, 0);
                                        do
                                        {
                                            product = randSet(products);
                                            if (!selected.Contains(product))
                                            {
                                                selected.Add(product);
                                                break;
                                            }
                                        } while (true);
                                        return
                                            new CartItem
                                            {
                                                ItemId = i,
                                                DatePlaced = randDateTime(10),
                                                ProductId = product.Item2,
                                                SessionId = sessionId,
                                                SupplierId = product.Item1,
                                                ProductCount = 1
                                            };
                                    });
                        }
                    );
                await context.CartItems.AddRangeAsync(carts);
                await context.SaveChangesAsync();
            }
        }

        public static async Task CreateReviews(IConfiguration config, string env)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BakeryHubContext>();
            optionsBuilder.UseSqlServer(config[$"ConnectionString:{env}"])
                .EnableSensitiveDataLogging();
            using (var context = new BakeryHubContext(optionsBuilder.Options))
            {
                var customers =
                    await
                        context.Customers.ToListAsync();
                var suppliers =
                    await
                        context.Suppliers.Include(s => s.Products).ToListAsync();
                var productReviews = 
                    genN(10000, i =>
                    {
                        var customer = randSet(customers);
                        var supplier = randSet(suppliers);
                        var product = randSet(supplier.Products);
                        return new ProductReview
                        {
                            SupplierId = product.SupplierId,
                            ProductId = product.ProductId,
                            Review = new Review
                            {
                                Date = randDateTime(100),
                                Feedback =
                                    randSet(new[] {
                                        "Best product",
                                        "Nice",
                                        "Ok",
                                        "Don't like"
                                        }),
                                IsAboutProduct = true,
                                Rating = randInt(3, 5),
                                TargetUserId = product.SupplierId,
                                UserId = customer.Id
                            }
                        };
                    }).ToList();
                await context.ProductReviews.AddRangeAsync(productReviews);
                await context.SaveChangesAsync();
            }
        }

        static async Task GenerateNotificationLogs(IConfiguration config, string env)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BakeryHubContext>();
            optionsBuilder.UseSqlServer(config[$"ConnectionString:{env}"])
                .EnableSensitiveDataLogging();
            using (var context = new BakeryHubContext(optionsBuilder.Options))
            {
                var contacts =
                    await (from c in context.Contacts select c).ToListAsync();

                var notifications =
                    genN(1000, i =>
                        {
                            var contact = randSet(contacts);
                            var status =
                                randBool(0.8) ?
                                    NotificationLog.NotificationDelivery.Failed :
                                    NotificationLog.NotificationDelivery.Delivered;
                            return
                                new NotificationLog
                                {
                                    ContactId = contact.ContactId,
                                    Date = randDateTime(1, DateTime.UtcNow.AddDays(2)),
                                    Status = status,
                                    ErrorMessage =
                                        status == NotificationLog.NotificationDelivery.Delivered ? null
                                        : randSet(
                                            new[]
                                            {
                                                "NetworkError",
                                                "ContactIsUnreachable",
                                                "InternalError"
                                            }),
                                    Text = "You order has been delivered",
                                    Subject = "Order status changed",
                                    UserId = contact.UserId
                                    //MessageId
                                };
                        }

                    );
                await context.NotificationLog.AddRangeAsync(notifications);
                await context.SaveChangesAsync();
            }
        }

        static async Task Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile("appsettings.json");
                var config = builder.Build();
                var env = "Development";
                //await TestSMS(config);
                //await TestEmail(config);

                //await CreateSupplier(config, env);
                //await CreateCustomers(config,
                //    new[]
                //    {
                //        "Alex",
                //        "Bob",
                //        "Kate",
                //        "Sonya",
                //        "Dmitry",
                //        "Alisha",
                //        "Dhruv"
                //    }, env);

                //await FixOrderPrices(config, env);

                //await CreateCartItems(config, env);
                //await CreateReviews(config, env);
                await GenerateNotificationLogs(config, env);
                System.Console.WriteLine("Done!"); 
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
