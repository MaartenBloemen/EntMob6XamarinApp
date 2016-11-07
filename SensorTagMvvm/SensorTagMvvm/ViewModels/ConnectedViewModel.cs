using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using DataProcessor;
using Java.Util;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Extensions;
using SensorTagMvvm.Services;
using Debug = System.Diagnostics.Debug;

namespace SensorTagMvvm.ViewModels
{
    class ConnectedViewModel : MvxViewModel
    {
        private Plugin.BLE.Abstractions.Contracts.IAdapter _adapter = Mvx.Resolve<Plugin.BLE.Abstractions.Contracts.IAdapter>();

        private IList<IService> _services;

        private Queue<ICharacteristic> _writeCharacteristics = new Queue<ICharacteristic>();
        private IList<ICharacteristic> _characteristics = new List<ICharacteristic>();

        public IList<double> TemperaturesList = new List<double>();
        public IList<double> HumidityList = new List<double>();
        public IList<double> BarometerList = new List<double>();
        public IList<double> OpticalList = new List<double>();

        public ConnectedViewModel()
        {
        }

        public override void Start()
        {
            base.Start();
            GetServices();
        }

        public void Init(DeviceParameters connectedDevice)
        {
            Debug.WriteLine(connectedDevice);
            DeviceId = connectedDevice.DeviceId;
            DeviceName = "Device name: " + connectedDevice.DeviceName;
        }

        private Guid _deviceId;

        public Guid DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; RaisePropertyChanged(() => DeviceId); }
        }

        private string _deviceName;

        public string DeviceName
        {
            get { return _deviceName; }
            set { _deviceName = value; RaisePropertyChanged(() => DeviceName); }
        }

        private string _temperatureData;

        public string TemperatureData
        {
            get { return _temperatureData; }
            set { _temperatureData = value; RaisePropertyChanged(() => TemperatureData); }
        }

        private string _humidityData;

        public string HumidityData
        {
            get { return _humidityData; }
            set { _humidityData = value; RaisePropertyChanged(() => HumidityData); }
        }

        private string _barometerData;

        public string BarometerData
        {
            get { return _barometerData; }
            set { _barometerData = value; RaisePropertyChanged(() => BarometerData); }
        }

        private string _opticalData;

        public string OpticalData
        {
            get { return _opticalData; }
            set { _opticalData = value; RaisePropertyChanged(() => OpticalData); }
        }

        private async void GetServices()
        {
            IDevice device = _adapter.ConnectedDevices.FirstOrDefault(d => d.Id.Equals(_deviceId));
            if (device != null)
            {
                _services = await device.GetServicesAsync();
            }
            GetTempService();
            GetHumidityService();
            GetBarometerService();
            GetOpticalService();
            WriteCharacteristics();
        }

        private async void GetTempService()
        {
            var service = _services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa00-0451-4000-b000-000000000000")));
            var writeCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa02-0451-4000-b000-000000000000"));
            _writeCharacteristics.Enqueue(writeCharacteristic);
            var readCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa01-0451-4000-b000-000000000000"));
            _characteristics.Add(readCharacteristic);
        }

        private async void GetHumidityService()
        {
            var service = _services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa20-0451-4000-b000-000000000000")));
            var writeCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa22-0451-4000-b000-000000000000"));
            _writeCharacteristics.Enqueue(writeCharacteristic);
            var readCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa21-0451-4000-b000-000000000000"));
            _characteristics.Add(readCharacteristic);
        }

        private async void GetBarometerService()
        {
            var service = _services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa40-0451-4000-b000-000000000000")));
            var writeCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa42-0451-4000-b000-000000000000"));
            _writeCharacteristics.Enqueue(writeCharacteristic);
            var readCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa41-0451-4000-b000-000000000000"));
            _characteristics.Add(readCharacteristic);
        }

        private async void GetOpticalService()
        {
            var service = _services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa70-0451-4000-b000-000000000000")));
            var writeCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa72-0451-4000-b000-000000000000"));
            _writeCharacteristics.Enqueue(writeCharacteristic);
            var readCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa71-0451-4000-b000-000000000000"));
            _characteristics.Add(readCharacteristic);
        }

        private async System.Threading.Tasks.Task<bool> TurnServiceOn(IService service, Guid characteristicId)
        {
            try
            {
                var characteristic = await service.GetCharacteristicAsync(characteristicId);
                await characteristic.WriteAsync(GetBytes("1"));
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        private static byte[] GetBytes(string text)
        {
            return text.Split(' ').Where(token => !string.IsNullOrEmpty(token)).Select(token => Convert.ToByte(token, 16)).ToArray();
        }

        private async void WriteCharacteristics()
        {
            while (_writeCharacteristics.Count != 0)
            {
                var characteristic = _writeCharacteristics.Dequeue();
                await characteristic.WriteAsync(GetBytes("1"));
            }
            new System.Threading.Thread(new System.Threading.ThreadStart(ReadCharacteristics)).Start();
        }

        private async void ReadCharacteristics()
        {
            while (true)
            {
                for (int i = 0; i < _characteristics.Count; i++)
                {
                    var characteristic = _characteristics[i];
                    var bytes = await characteristic.ReadAsync();
                    switch (i)
                    {
                        case 0:
                            var t = Converter.AmbientTemperature(bytes);
                            TemperatureData = "Temp: " + Math.Round(t, 2);
                            TemperaturesList.Add(t);
                            break;
                        case 1:
                            var h = Converter.Humidity(bytes);
                            HumidityData = "Humidity:" + Math.Round(h, 2); ;
                            HumidityList.Add(h);
                            break;
                        case 2:
                            var b = Converter.Barometer(bytes);
                            BarometerData = "Barometer: " + Math.Round(b, 2); ;
                            BarometerList.Add(b);
                            break;
                        case 3:
                            var o = Converter.Lux(bytes);
                            OpticalData = "Lux: " + Math.Round(o, 2); ;
                            OpticalList.Add(o);
                            break;
                    }
                }
            }
        }
    }
}