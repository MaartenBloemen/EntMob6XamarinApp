using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    class Converter
    {
        public static double AmbientTemperature(byte[] bytes)
        {
            //vb array
            //byte[] bytes = new byte[] { 0x04, 0x0A, 0x04, 0x0C };

            var ambientTemp = BitConverter.ToUInt16(bytes, 2) / 128.0;
            return ambientTemp;
        }

        public static double IRTemperature(byte[] bytes)
        {
            //vb array
            //byte[] bytes = new byte[] { 0x04, 0x0A, 0x04, 0x0C };

            var IRTemp = BitConverter.ToUInt16(bytes, 0) / 128.0;
            return IRTemp;
        }

        public static double Humidity(byte[] bytes)
        {
            //vb aray
            //byte[] humidityBytes = new byte[] { 0x78, 0x63, 0x50, 0x94 };
            var humidity = ((double)BitConverter.ToUInt16(bytes, 2) / 65536) * 100;

            return humidity;
        }

        public static double Barometer(byte[] bytes)
        {
            //vb array
            //byte[] barometerBytes = new byte[] { 0x00, 0x00, 0xD4, 0x09 };
            //byte[] barometerBytes2 = new byte[] { 0x00, 0x8C, 0x8B, 0x01 }; <-- enkel deze wordt in formule gebruikt

            var airPressure = ((bytes[3] + (bytes[4] << 8) + (bytes[5] << 16)) / 100.0f) - 100;

            return airPressure;
        }


        public static double Lux(byte[] bytes)
        {
            //VB array
            // byte[] luxBytes = new byte[] { 0x00, 0x00, 0x4C, 0x49 };

            System.UInt16 e, m;
            m = (System.UInt16)(BitConverter.ToUInt16(bytes, 2) & 0x0FFF);
            e = (System.UInt16)((BitConverter.ToUInt16(bytes, 2) & 0xF000) >> 12);

            return (m * (0.01 * Math.Pow(2.0, e)));
        }
    }
}
