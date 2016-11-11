using System;
using System.Collections.Generic;
using MvvmCross.Platform;
using MvvmCross.Test.Core;
using NUnit.Framework;
using SensorTagMvvm.ViewModels;
using UnitTesting;
using Moq;

namespace UnitTesting
{
    /*
     *TODO: lists like temperature list need to be lists of models not double.
     *TODO: Unit test for these lists need to written after this is done. Only one unit test done this far.
     */
    [TestFixture]
    public class ConnectedViewModelTest : MvxTest
    {
        private ConnectedViewModel _connectedViewModel;
        [Test]
        public void TestConnectedViewModelDeviceName()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var viewModel = new ConnectedViewModel { DeviceName = "Test" };

            Assert.AreEqual("Test", viewModel.DeviceName);
        }

        [Test]
        public void TestConnectedViewModelDeviceId()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();
            var viewModel = new ConnectedViewModel { DeviceId = Guid.Parse("41697849-d8b1-47cc-b236-f674fb721b35") };


            Assert.AreEqual(Guid.Parse("41697849-d8b1-47cc-b236-f674fb721b35"), viewModel.DeviceId);
        }

        [Test]
        public void TestConnectedViewModelTemperatureList()
        {
            ClearAll();
            var mockNavigation = CreateMockNavigation();

            var viewModel = new ConnectedViewModel { TemperaturesList = new List<double> { 10, 20, 30, 40, 50 } };


            Assert.AreEqual(5, viewModel.TemperaturesList.Count);
            Assert.AreEqual(10, viewModel.TemperaturesList[0]);
        }
    }
}