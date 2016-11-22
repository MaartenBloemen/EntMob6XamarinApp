using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorTagMvvm.Domain;

namespace SensorTagMvvm.DAL
{
    public interface ISensorTagRepository
    {
        void PostTemperatureData(List<Temperature> temperatures);
        void PostHumidityData(List<Humidity> humidities);
        void PostBarometerData(List<AirPressure> airPressures);
        void PostOpticalData(List<Brightness> brightnesses);
    }
}
