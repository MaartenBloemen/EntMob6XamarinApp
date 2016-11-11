using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using NUnit.Framework;
using SensorTagMvvm.ViewModels;

namespace SensortagUnitTesting
{
    [TestFixture]
    public class ConnectedViewModelTest
    {
        private ConnectedViewModel _connectedViewModel;
        [Test]
        public void TestConnectedViewModel()
        {
            _connectedViewModel = new ConnectedViewModel();
        }
    }
}