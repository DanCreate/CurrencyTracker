using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CurrencyTracker.Models;

namespace CurrencyTracker.Data
{
    public class CurrencyTrackerContext : DbContext
    {
        public CurrencyTrackerContext (DbContextOptions<CurrencyTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<CurrencyTracker.Models.Rates> Rates { get; set; } = default!;
    }
}
