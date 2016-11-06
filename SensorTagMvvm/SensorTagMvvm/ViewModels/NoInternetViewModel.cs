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
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace SensorTagMvvm.ViewModels
{
    public class NoInternetViewModel : MvxViewModel
    {
        private readonly IConnectivity _connectivity = CrossConnectivity.Current;
        public NoInternetViewModel()
        {
        }

        public override void Start()
        {
            base.Start();
            CheckConnectivity();
        }

        private int _isConnected;

        public int IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; RaisePropertyChanged(()=>IsConnected); }
        }

        private int _internetAccess;

        public int InternetAccess
        {
            get { return _internetAccess; }
            set { _internetAccess = value; RaisePropertyChanged(() => InternetAccess); }
        }

        private void CheckConnectivity()
        {
            if (_connectivity.IsConnected)
            {
                IsConnected = Resource.Drawable.green_square;
                CheckInternetAccess();
            }
            else
            {
                IsConnected = Resource.Drawable.red_square;
                InternetAccess = Resource.Drawable.red_square;
            }
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (args.IsConnected)
                {
                    IsConnected = Resource.Drawable.green_square;
                    CheckInternetAccess();
                }
                else
                {
                    IsConnected = Resource.Drawable.red_square;
                    InternetAccess = Resource.Drawable.red_square;
                }
            };
        }

        private async void CheckInternetAccess()
        {
            if (await _connectivity.IsRemoteReachable("google.com"))
            {
                InternetAccess = Resource.Drawable.green_square;
            }
            else
            {
                InternetAccess = Resource.Drawable.red_square;
            }
        }
    }
}