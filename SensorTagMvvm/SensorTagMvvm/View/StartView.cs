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

namespace SensorTagMvvm.View
{
    [Activity(Label = "Sensortag", MainLauncher = true)]
    public class StartView : MvxActivity
    {
        public new StartViewModel ViewModel
        {
            get { return (StartViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.start_screen);
        }
    }
}