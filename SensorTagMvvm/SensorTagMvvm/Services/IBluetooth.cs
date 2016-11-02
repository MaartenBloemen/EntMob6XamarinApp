using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.BLE.Abstractions.Contracts;

namespace SensorTagMvvm.Services
{
    public interface IBluetooth
    {
        System.Threading.Tasks.Task<List<IDevice>> GetDevices(Plugin.BLE.Abstractions.Contracts.IAdapter adapter);
        BluetoothState GetBluetoothState(IBluetoothLE bluetoothLe);
    }
}