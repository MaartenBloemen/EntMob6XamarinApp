using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
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

        public StartViewModel(IBluetooth bluetooth)
        {
            _bluetooth = bluetooth;
        }

        public override void Start()
        {
            base.Start();
            GetBluetoothStatus();
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
                            Debug.WriteLine("3");
                            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                            if (status != PermissionStatus.Granted)
                            {
                                Debug.WriteLine("4");
                                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                                {
                                    Debug.WriteLine("5");
                                }

                                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                                status = results[Permission.Location];
                                Debug.WriteLine(status.ToString());
                            }

                            if (status == PermissionStatus.Granted)
                            {
                                Debug.WriteLine("6");
                                ScanStatus = "Scanning for devices...";
                                GetBluetoothDevices();
                            }
                            else if (status != PermissionStatus.Unknown)
                            {
                                Debug.WriteLine("7");
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

        public IMvxCommand ConnectCommand
        {
            get { return new MvxCommand<IDevice>(Connect); }
        }

        private async void Connect(IDevice device)
        {
            try
            {
                await adapter.ConnectToDeviceAsync(device);
                Debug.WriteLine("Connected");
                ShowViewModel<ConnectedViewModel>(new DeviceParameters() { DeviceId = device.Id, DeviceName = device.Name });
            }
            catch (DeviceConnectionException e)
            {
                Debug.WriteLine("Something went wrong...");
            }
        }

        private string _bluetoothStatus;

        public string BluetoothStatus
        {
            get { return _bluetoothStatus; }
            set { _bluetoothStatus = value; RaisePropertyChanged(() => BluetoothStatus); }

        }

        private List<IDevice> _deviceList = new List<IDevice>();

        public List<IDevice> DeviceList
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
            List<IDevice> deviceList = new List<IDevice>();
            adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);
            await adapter.StartScanningForDevicesAsync();
            deviceList.RemoveAll(device => (device.Name == null) && !(device.Id.ToString().Contains("b0b448")));
            DeviceList = deviceList.OrderBy(d => d.Rssi).ToList();
            ScanStatus = deviceList.Count == 0 ? "No devices found" : "Available devices:";
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
    }
}