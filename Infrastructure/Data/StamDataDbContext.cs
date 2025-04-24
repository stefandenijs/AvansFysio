using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Webservice;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StamDataDbContext : DbContext
    {
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<TreatmentInfo> Treatments { get; set; }

        public StamDataDbContext(DbContextOptions<StamDataDbContext> contextOptions) : base(contextOptions)
        {

        }
    }
}
