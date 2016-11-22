using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorTagMvvm.Domain
{
    public class Temperature
    {
        public string ID { get; set; }
        public float Value { get; set; }
        public DateTime Measured { get; set; }
    }
}
