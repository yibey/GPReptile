using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GPReptile.Models;

namespace GPReptile.Data
{
    public class GPContext : DbContext
    {
        public GPContext (DbContextOptions<GPContext> options)
            : base(options)
        {
        }

        public DbSet<GPReptile.Models.DayTransact> DayTransact { get; set; }

        public DbSet<GPReptile.Models.Share> Share { get; set; }
    }
}
