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
using Android.Views;
using Android.Widget;
using DataProcessor;
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
        private Plugin.BLE.Abstractions.Contracts.IAdapter adapter = Mvx.Resolve<Plugin.BLE.Abstractions.Contracts.IAdapter>();
        private IList<IService> services;

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
            IDevice device = adapter.ConnectedDevices.FirstOrDefault(d => d.Id.Equals(_deviceId));
            if (device != null)
            {
                services = await device.GetServicesAsync();
            }
            new System.Threading.Thread(new System.Threading.ThreadStart(GetTempService)).Start();
            new System.Threading.Thread(new System.Threading.ThreadStart(GetHumidityService)).Start();
            new System.Threading.Thread(new System.Threading.ThreadStart(GetBarometerService)).Start();
            new System.Threading.Thread(new System.Threading.ThreadStart(GetOpticalService)).Start();
        }

        private async void GetTempService()
        {
            var service = services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa00-0451-4000-b000-000000000000")));
            var done = await TurnServiceOn(service, Guid.Parse("f000aa02-0451-4000-b000-000000000000"));
            if (done)
            {
                var characteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa01-0451-4000-b000-000000000000"));
                characteristic.ValueUpdated += (o, args) =>
                {
                    var bytes = args.Characteristic.Value;
                    TemperatureData = "Temp=" + Converter.IRTemperature(bytes);
                    TemperaturesList.Add(Converter.IRTemperature(bytes));
                };

                await characteristic.StartUpdatesAsync();
            }
        }

        private async void GetHumidityService()
        {
            var service = services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa20-0451-4000-b000-000000000000")));
            var done = await TurnServiceOn(service, Guid.Parse("f000aa22-0451-4000-b000-000000000000"));
            if (done)
            {
                var characteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa21-0451-4000-b000-000000000000"));
                characteristic.ValueUpdated += (o, args) =>
                {
                    var bytes = args.Characteristic.Value;
                    HumidityData = "Humidity=" + Converter.Humidity(bytes);
                    HumidityList.Add(Converter.Humidity(bytes));
                };

                await characteristic.StartUpdatesAsync();
            }
        }

        private async void GetBarometerService()
        {
            var service = services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa40-0451-4000-b000-000000000000")));
            var done = await TurnServiceOn(service, Guid.Parse("f000aa42-0451-4000-b000-000000000000"));
            Debug.WriteLine("test");
            if (done)
            {
                var characteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa41-0451-4000-b000-000000000000"));
                characteristic.ValueUpdated += (o, args) =>
                {
                    var bytes = args.Characteristic.Value;
                    BarometerData = "Barometer=" + Converter.Barometer(bytes);
                    BarometerList.Add(Converter.Barometer(bytes));
                };

                await characteristic.StartUpdatesAsync();
            }
        }

        private async void GetOpticalService()
        {
            var service = services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa70-0451-4000-b000-000000000000")));
            var done = await TurnServiceOn(service, Guid.Parse("f000aa72-0451-4000-b000-000000000000"));
            if (done)
            {
                var characteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa71-0451-4000-b000-000000000000"));
                characteristic.ValueUpdated += (o, args) =>
                {
                    var bytes = args.Characteristic.Value;
                    OpticalData = "Lux=" + Converter.Lux(bytes);
                    OpticalList.Add(Converter.Lux(bytes));
                };

                await characteristic.StartUpdatesAsync();
            }
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
    }
}