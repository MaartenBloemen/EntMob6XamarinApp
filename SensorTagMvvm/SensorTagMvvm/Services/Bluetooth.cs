using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace SensorTagMvvm.Services
{
    public class Bluetooth : IBluetooth
    {
        private Plugin.BLE.Abstractions.Contracts.IAdapter _adapter = CrossBluetoothLE.Current.Adapter;
        private BluetoothAdapter _mBluetoothAdapter;

        public async System.Threading.Tasks.Task<List<IDevice>> GetDevices(Plugin.BLE.Abstractions.Contracts.IAdapter adapter)
        {
            List<IDevice> deviceList = new List<IDevice>();
            adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);
            await adapter.StartScanningForDevicesAsync();
            deviceList.RemoveAll(device => (device.Name == null) && !(device.Id.ToString().Contains("b0b448")));
            return deviceList;
        }

        public BluetoothState GetBluetoothState(IBluetoothLE bluetoothLe)
        {
            var state = bluetoothLe.State;
            return state;
        }
    }
}