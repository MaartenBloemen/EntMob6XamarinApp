using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
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
        public ConnectedViewModel()
        {
        }

        public override void Start()
        {
            base.Start();
            GetServices();
        }

        public void Init(DeviceParameters connected_device)
        {
            Debug.WriteLine(connected_device);
            DeviceId = connected_device.DeviceId;
            DeviceName = "Device name: " + connected_device.DeviceName;
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

        private async void GetServices()
        {
            IDevice device = adapter.ConnectedDevices.FirstOrDefault(d => d.Id.Equals(_deviceId));
            if (device != null)
            {
                services = await device.GetServicesAsync();
            }
            //GetTempService();
            //GetBarometerService();
            GetHumidityService();
            //GetOpticalService();
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
                    TemperatureData = "Humidity=" + Converter.Humidity(bytes);
                };

                await characteristic.StartUpdatesAsync();
            }
        }

        private async void GetBarometerService()
        {
            var service = services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa40-0451-4000-b000-000000000000")));
            var done = await TurnServiceOn(service, Guid.Parse("f000aa42-0451-4000-b000-000000000000"));
            if (done)
            {
                var characteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa41-0451-4000-b000-000000000000"));
                characteristic.ValueUpdated += (o, args) =>
                {
                    var bytes = args.Characteristic.Value;
                    TemperatureData = "Barometer=" + Converter.Barometer(bytes);
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
                    TemperatureData = "Lux=" + Converter.Lux(bytes);
                };

                await characteristic.StartUpdatesAsync();
            }
        }

        private async System.Threading.Tasks.Task<bool> TurnServiceOn(IService Service, Guid CharacteristicId)
        {
            try
            {
                var characteristic = await Service.GetCharacteristicAsync(CharacteristicId);
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