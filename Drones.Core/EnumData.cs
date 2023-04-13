using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Core
{
    public enum DroneModelEnum { Lightweight, Middleweight, Cruiserweight, Heavyweight }
    public enum DroneStateEnum { IDLE, LOADING, LOADED, DELIVERING, DELIVERED, RETURNING }
}
