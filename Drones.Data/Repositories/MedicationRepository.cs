﻿using Drones.Core.Repositories;
using Drones.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Data.Repositories
{
    public class MedicationRepository : Repository<Medication>, IMedicationRepository
    {
        public MedicationRepository(DronesContext context)
            : base(context)
        { }

        private DronesContext DronesContext
        {
            get { return Context as DronesContext; }
        }
    }
}