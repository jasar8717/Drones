using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Entities.Models
{
    public partial class DronesContext : DbContext
    {
        public DronesContext()
        {
        }

        public DronesContext(DbContextOptions<DronesContext> options) : base(options)
        {
        }


    }
}
