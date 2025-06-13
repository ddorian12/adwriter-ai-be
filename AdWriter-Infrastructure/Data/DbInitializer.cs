using AdWriter_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.GenerationPacks.Any())
            {
                context.GenerationPacks.AddRange(new[]
                {
                new GenerationPack { Name = "10 generări", Generations = 10, PriceRon = 15 },
                new GenerationPack { Name = "25 generări", Generations = 25, PriceRon = 30 },
                new GenerationPack { Name = "50 generări", Generations = 50, PriceRon = 50 }
            });

                context.SaveChanges();
            }
        }
    }

}
