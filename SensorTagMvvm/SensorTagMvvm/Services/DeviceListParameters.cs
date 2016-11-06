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
    public class DeviceListParameters
    {
        public IDevice Device { get; set; }
        public string DeviceName { get; set; }
        public int DeviceRssi { get; set; }
    }
}