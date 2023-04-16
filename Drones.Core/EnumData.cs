using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Core
{
    public enum DroneModelEnum 
    {
        [Description("Lightweight")]
        Lightweight,
        [Description("Middleweight")]
        Middleweight,
        [Description("Cruiserweight")]
        Cruiserweight,
        [Description("Heavyweight")]
        Heavyweight
    }
    public enum DroneStateEnum 
    {
        [Description("IDLE")]
        IDLE,
        [Description("LOADING")]
        LOADING,
        [Description("LOADED")]
        LOADED,
        [Description("DELIVERING")]
        DELIVERING,
        [Description("DELIVERED")]
        DELIVERED,
        [Description("RETURNING")]
        RETURNING
    }
}
