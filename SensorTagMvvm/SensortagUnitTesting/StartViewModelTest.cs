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
using NUnit.Framework;
using SensorTagMvvm.Services;
using SensorTagMvvm.ViewModels;

namespace SensortagUnitTesting
{
    [TestFixture]
    public class StartViewModelTest
    {
        private StartViewModel _startViewModel;
        [Test]
        public void TestStartViewModel()
        {
            _startViewModel = new StartViewModel(new Bluetooth());
            Assert.AreEqual(4, 4);
        }
    }
}