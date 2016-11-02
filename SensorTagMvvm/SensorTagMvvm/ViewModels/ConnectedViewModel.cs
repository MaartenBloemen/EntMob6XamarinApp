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
            GetTempService();
        }

        private async void GetTempService()
        {
            var service = services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa00-0451-4000-b000-000000000000")));
            var characteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa01-0451-4000-b000-000000000000"));
            await characteristic.ReadAsync();
            string CharacteristicValue = characteristic?.Value.ToHexString().Replace("-", " ");
            var bytes = GetBytes(CharacteristicValue);
            Debug.WriteLine("test");
        }

        private async void GetHumidityService()
        {
            var service = services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa20-0451-4000-b000-000000000000")));
            var characteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa21-0451-4000-b000-000000000000"));
            var bytes = await characteristic.ReadAsync();
        }

        private async void GetBarometerService()
        {
            var service = services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa40-0451-4000-b000-000000000000")));
            var characteristic = await service.GetCharacteristicAsync(Guid.Parse("f000aa41-0451-4000-b000-000000000000"));
            var bytes = await characteristic.ReadAsync();
        }

        private async void GetOpticalService()
        {
            var service = services.FirstOrDefault(s => s.Id.Equals(Guid.Parse("f000aa70-0451-4000-b000-000000000000")));
        }

        private static byte[] GetBytes(string text)
        {
            return text.Split(' ').Where(token => !string.IsNullOrEmpty(token)).Select(token => Convert.ToByte(token, 16)).ToArray();
        }
    }
}