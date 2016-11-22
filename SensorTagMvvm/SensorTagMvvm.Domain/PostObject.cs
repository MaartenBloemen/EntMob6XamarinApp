using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SensorTagMvvm.Domain
{
    public class PostObject<T>
    {
        [JsonProperty("value")]
        public T Value { get; set; }
        [JsonProperty("timeStamp")]
        public long TimeStamp{ get; set; }

        public PostObject(T value, long timeStamp)
        {
            this.Value = value;
            this.TimeStamp = timeStamp;
        }
    }
}
