using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SensorTagMvvm.Services;
using Debug = System.Diagnostics.Debug;

namespace SensorTagMvvm.ViewModels
{
    public class StartViewModel : MvxViewModel
    {
        private readonly IBluetooth _bluetooth;

        private Plugin.BLE.Abstractions.Contracts.IAdapter adapter = Mvx.Resolve<Plugin.BLE.Abstractions.Contracts.IAdapter>();
        private BluetoothAdapter mBluetoothAdapter;


        private readonly IConnectivity _connectivity = CrossConnectivity.Current;

        public StartViewModel(IBluetooth bluetooth)
        {
            _bluetooth = bluetooth;
        }

        public override void Start()
        {
            base.Start();
            TestInternet();
        }

        public IMvxCommand ScanDevicesCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    if (_bluetooth.GetBluetoothState(CrossBluetoothLE.Current) == BluetoothState.On)
                    {
                        try
                        {
                            //Debug.WriteLine("3");
                            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                            if (status != PermissionStatus.Granted)
                            {
                                //Debug.WriteLine("4");
                                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                                {
                                    //Debug.WriteLine("5");
                                }

                                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                                status = results[Permission.Location];
                                Debug.WriteLine(status.ToString());
                            }

                            if (status == PermissionStatus.Granted)
                            {
                                //Debug.WriteLine("6");
                                ScanStatus = "Scanning for devices...";
                                GetBluetoothDevices();
                            }
                            else if (status != PermissionStatus.Unknown)
                            {
                                //Debug.WriteLine("7");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Something went wrong");
                        }
                    }
                    else
                    {
                        if (await Mvx.Resolve<IUserInteraction>().ConfirmAsync("Bluetooth is off. Do you want to turn Bluetooth on?"))
                        {
                            var mBluetoothAdapter = Android.Bluetooth.BluetoothAdapter.DefaultAdapter;
                            mBluetoothAdapter.Enable();
                        }
                    }
                });
            }
        }

        public IMvxCommand ConnectCommand => new MvxCommand<DeviceListParameters>(Connect);

        private async void Connect(DeviceListParameters parameters)
        {
            IDevice device = parameters.Device;
            if (
                await
                    Mvx.Resolve<IUserInteraction>().ConfirmAsync("Do you want to connect to this device?"))
            {
                try
                {
                    await adapter.ConnectToDeviceAsync(device);
                    //Debug.WriteLine("Connected");
                    ShowViewModel<ConnectedViewModel>(new DeviceParameters()
                    {
                        DeviceId = device.Id,
                        DeviceName = device.Name
                    });
                }
                catch (DeviceConnectionException e)
                {
                    Debug.WriteLine("Something went wrong...");
                }
            }
        }

        private string _bluetoothStatus;

        public string BluetoothStatus
        {
            get { return _bluetoothStatus; }
            set { _bluetoothStatus = value; RaisePropertyChanged(() => BluetoothStatus); }

        }

        private List<DeviceListParameters> _deviceList = new List<DeviceListParameters>();

        public List<DeviceListParameters> DeviceList
        {
            get { return _deviceList; }
            set { _deviceList = value; RaisePropertyChanged(() => DeviceList); }
        }

        private string _scanStatus;

        public string ScanStatus
        {
            get { return _scanStatus; }
            set { _scanStatus = value; RaisePropertyChanged(() => ScanStatus); }
        }

        private async void GetBluetoothDevices()
        {
            List<DeviceListParameters> deviceList = new List<DeviceListParameters>();
            adapter.DeviceDiscovered += (s, a) => deviceList.Add(new DeviceListParameters() { Device = a.Device, DeviceName = a.Device.Name, DeviceRssi = GetRssi(a.Device.Rssi) });
            await adapter.StartScanningForDevicesAsync();
            deviceList.RemoveAll(device => (device.DeviceName == null) && !(device.Device.Id.ToString().Contains("b0b448")));
            DeviceList = deviceList.OrderBy(d => d.Device.Rssi).ToList();
            ScanStatus = deviceList.Count == 0 ? "No devices found" : "Available devices:";
        }

        private int GetRssi(int rssi)
        {
            var quality = 2 * (rssi + 100);
            if (quality > 90)
            {
                return Resource.Drawable.bars_4;
            }
            if (quality < 90 && quality > 80)
            {
                return Resource.Drawable.bars_3;
            }
            if (quality < 80 && quality > 60)
            {
                return Resource.Drawable.bars_2;
            }
            if (quality < 60 && quality > 40)
            {
                return Resource.Drawable.bars_1;
            }
            return Resource.Drawable.bars_0;
        }

        private void GetBluetoothStatus()
        {
            var ble = Mvx.Resolve<IBluetoothLE>();
            BluetoothStatus = "Bluetooth: " + _bluetooth.GetBluetoothState(ble);
            ble.StateChanged += (s, e) =>
            {
                BluetoothStatus = "Bluetooth: " + e.NewState.ToString();
            };
        }

        private async void TestInternet()
        {
            if ((_connectivity.IsConnected) && (await _connectivity.IsRemoteReachable("google.com")))
            {
                GetBluetoothStatus();
            }
            else
            {
                ShowViewModel<NoInternetViewModel>();
            }
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (!args.IsConnected)
                {
                    ShowViewModel<NoInternetViewModel>();
                }
            };
        }
    }
}