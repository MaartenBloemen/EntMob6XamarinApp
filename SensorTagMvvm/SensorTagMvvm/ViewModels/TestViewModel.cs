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
using Debug = System.Diagnostics.Debug;

namespace SensorTagMvvm.ViewModels
{
    public class TestViewModel : MvxViewModel
    {
        public override void Start()
        {
            base.Start();
        }

        public IMvxCommand RefreshCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    IsRefreshing = true;
                    for (var i = 0; i < 1000; i++)
                    {
                        Debug.WriteLine(i);
                        Test = "Counter: " + i;
                    }
                    IsRefreshing = false;
                });
            }
        }

        public bool IsRefreshing { get; set; }

        private string _test;

        public string Test
        {
            get { return _test; }
            set { _test = value; RaisePropertyChanged(() => Test); }
        }
    }
}