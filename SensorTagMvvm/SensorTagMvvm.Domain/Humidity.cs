using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SensorTagMvvm.Domain
{
    public class Humidity
    {
     
        public string ID { get; set; }
        public float Percentage { get; set; }
        public DateTime Measured { get; set; }
    }
}
