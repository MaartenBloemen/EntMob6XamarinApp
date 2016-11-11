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
using MvvmCross.Droid.Views;
using SensorTagMvvm.ViewModels;
using Debug = System.Diagnostics.Debug;

namespace SensorTagMvvm.View
{
    [Activity(Label = "Device connected")]
    class ConnectedView : MvxActivity
    {
        public new ConnectedViewModel ViewModel
        {
            get { return (ConnectedViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.device_connected);
        }
    }
}