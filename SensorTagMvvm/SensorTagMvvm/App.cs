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
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using SensorTagMvvm.Services;
using SensorTagMvvm.ViewModels;

namespace SensorTagMvvm
{
    public class App : MvxApplication
    {
        public App()
        {
            Mvx.RegisterType<IBluetooth, Bluetooth>();
            Mvx.RegisterType<IUserInteraction, UserInteraction>();
            Mvx.RegisterSingleton<IMvxAppStart>(new MvxAppStart<StartViewModel>());
        }
    }
}