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

namespace SensorTagMvvm.Services
{
    public class DeviceParameters
    {
        public Guid DeviceId { get; set; }
        public string DeviceName { get; set; }
    }
}